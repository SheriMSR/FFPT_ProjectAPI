using AutoMapper;
using FFPT_Project.Data.Entity;
using FFPT_Project.Data.UnitOfWork;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Exceptions;
using Hangfire;
using IronBarCode;
using Microsoft.EntityFrameworkCore;
using Reso.Sdk.Core.Custom;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static FFPT_Project.Service.Helpers.Enum;

namespace FFPT_Project.Service.Service
{
    public interface IOrderService
    {
        //Task<bool> CreateMailMessage(string mail, string orderName);
        Task<List<OrderResponse>> CreateOrder(CreateOrderRequest request);
    }
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateMailMessage(int CustomerId, string orderName)
        {
            var myBarcode = BarcodeWriter.CreateBarcode(orderName, BarcodeWriterEncoding.Code128);
            Image myBarcodeImage = myBarcode.Image;

            var customer = _unitOfWork.Repository<Customer>().GetAll()
                            .FirstOrDefault(x => x.Id == CustomerId);

            bool success = false;
            string to = customer.Email;
            string from = "ffpt.ffood@gmail.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Đơn hàng " + orderName + " Đơn hàng của bạn đã được shipper tiếp nhận";
            message.Body = @"Vui lòng đưa mã QR này cho shipper để xác nhận đã giao hàng thành công nhé!" + myBarcodeImage;
            // $"<font style=\"vertical-align: inherit\">Cảm ơn {orderInfo.Customer.Name} đã đặt món</font></font><br />  ;
            message.IsBodyHtml = true;
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("mytdvse151417@fpt.edu.vn", "070301000119");
            SmtpServer.EnableSsl = true;

            try
            {
                SmtpServer.Send(message);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                throw new Exception(ex.Message);
            }
            return success;
        }
        public async Task<List<OrderResponse>> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                var order = _mapper.Map<CreateOrderRequest, Order>(request);
                var result = new List<OrderResponse>();
                
                #region checkDeliveryPhone
                string phone = request.DeliveryPhone;
                if (phone != null)
                {
                    var check = CheckVNPhoneEmail(phone);
                    if (!check)
                    { 
                        throw new CrudException(HttpStatusCode.BadRequest, "Wrong Phone", phone.ToString());
                    }
                }                
                #endregion

                HashSet<int> listStore = new HashSet<int>();
                foreach (var detail in request.OrderDetails)
                {
                    listStore.Add(detail.SupplierStoreId);
                }

                if (request.OrderType == (int)OrderTypeEnum.Delivery)
                {                   
                    order.ShippingFee = listStore.Count() * 2000;
                }
                
                foreach(var item in listStore)
                {
                    
                    string refixOrderName = "FFPT";
                    var orderCount = _unitOfWork.Repository<Order>().GetAll()
                        .Where(x => ((DateTime)x.CheckInDate).Date.Equals(DateTime.Now.Date)).Count() + 1;
                    order.OrderName = refixOrderName + "-" + orderCount.ToString().PadLeft(3, '0');

                    foreach (var detail in request.OrderDetails)
                    {
                        if (detail.SupplierStoreId == item)
                        {
                            order.TotalAmount += (double)detail.FinalAmount;
                            var orderDetail = _mapper.Map<OrderDetailRequest, OrderDetail>(detail);
                            order.OrderDetails.Add(orderDetail);
                        }
                    }

                    order.CheckInDate = DateTime.Now;
                    order.OrderStatus = (int)OrderStatusEnum.Pending;             
                    await _unitOfWork.Repository<Order>().InsertAsync(order);
                    await _unitOfWork.CommitAsync();

                    var miniOrder = _mapper.Map<Order, OrderResponse>(order);

                    result.Add(miniOrder);

                    try
                    {
                        CreateMailMessage(request.CustomerId, order.OrderName);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
                return result;
                
                return null;
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create Order Error!!!", e.InnerException?.Message);
            }
        }
        public static bool CheckVNPhoneEmail(string phoneNumber)
        {
            string strRegex = @"(^(0)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$)";
            Regex re = new Regex(strRegex);
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (re.IsMatch(phoneNumber))
            {
                return true;
            }
            else
                return false;
        }
    }
}

