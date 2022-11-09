using AutoMapper;
using AutoMapper.QueryableExtensions;
using Castle.Core.Resource;
using Chilkat;
using FFPT_Project.Data.Entity;
using FFPT_Project.Data.UnitOfWork;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Exceptions;
using FFPT_Project.Service.Helpers;
using Hangfire;
using IronBarCode;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Reso.Sdk.Core.Custom;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
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
        Task<PagedResults<OrderResponse>> GetOrderByOrderStatus(OrderStatusEnum orderStatus, int customerId, PagingRequest paging);
        Task<PagedResults<OrderResponse>> GetOrders(PagingRequest paging);
        Task<OrderResponse> UpdateOrderStatus (int orderId, OrderStatusEnum orderStatus);
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

        public async Task<bool> CreateMailMessage(string customerEmail, string orderName)
        {
            var myBarcode = BarcodeWriter.CreateBarcode(orderName, BarcodeWriterEncoding.Code128);
            Image myBarcodeImage = myBarcode.Image;

            bool success = false;
            string to = customerEmail;
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
                var customer = _unitOfWork.Repository<Customer>().GetAll()
                .FirstOrDefault(x => x.Id == request.CustomerId);

                if (request.DeliveryPhone != null)
                {
                    var check = CheckVNPhoneEmail(request.DeliveryPhone);
                    if (check)
                    {
                        order.DeliveryPhone = request.DeliveryPhone;
                    }
                }
                else
                {
                    order.DeliveryPhone = customer.Phone;
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
                        CreateMailMessage(customer.Email, order.OrderName);
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

        public async Task<PagedResults<OrderResponse>> GetOrderByOrderStatus(OrderStatusEnum orderStatus, int customerId, PagingRequest paging)
        {
            try
            {
                var order = await _unitOfWork.Repository<Order>().GetAll()
                            .Where(x => x.OrderStatus == (int)orderStatus
                            && x.Customer.Id == customerId)
                            .ProjectTo<OrderResponse>(_mapper.ConfigurationProvider)
                            .ToListAsync();
                return PageHelper<OrderResponse>.Paging(order, paging.Page, paging.PageSize);
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Error", ex.Message);
            }
        }

        public async Task<PagedResults<OrderResponse>> GetOrders(PagingRequest paging)
        {
            try
            {
                var order = await _unitOfWork.Repository<Order>().GetAll()
                            .ProjectTo<OrderResponse>(_mapper.ConfigurationProvider)
                            .ToListAsync();
                return PageHelper<OrderResponse>.Paging(order, paging.Page, paging.PageSize);
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Error", ex.Message);
            }
        }

        public async Task<OrderResponse> UpdateOrderStatus(int orderId, OrderStatusEnum orderStatus)
        {
            try
            {
                var order = await _unitOfWork.Repository<Order>().GetAll()
                            .Where(x => x.Id == orderId)
                            .FirstOrDefaultAsync();
                order.OrderStatus = (int)orderStatus;

                await _unitOfWork.Repository<Order>().UpdateDetached(order);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<OrderResponse>(order);
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Error", e.Message);
            }
        }

    }
}

