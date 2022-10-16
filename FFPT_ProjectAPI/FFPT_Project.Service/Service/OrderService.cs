using AutoMapper;
using FFPT_Project.Data.Entity;
using FFPT_Project.Data.UnitOfWork;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Exceptions;
using Hangfire;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static FFPT_Project.Service.Helpers.Enum;

namespace FFPT_Project.Service.Service
{
    public interface IOrderService
    {
        Task<bool> CreateMailMessage(string mail);
        Task<OrderResponse> CreateOrder(CreateOrderRequest request);
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

        //    public static QRCodeData GenerateQRCode(int orderId)
        //    {
        //        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //        QRCodeData qrCodeData = qrGenerator.CreateQrCode(orderId.ToString(), QRCodeGenerator.ECCLevel.Q);
        //        var qrCode = new QRCode(qrCodeData);
        //        Bitmap qrCodeImage = qrCode.GetGraphic(20);

        //        Set color by using Color-class types
        //        qrCodeImage = qrCode.GetGraphic(20, Color.DarkRed, Color.PaleGreen, true);
        //        Set color by using HTML hex color notation
        //        qrCodeImage = qrCode.GetGraphic(20, "#000ff0", "#0ff000");

        //PayloadGenerator.WiFi wifiPayload = new PayloadGenerator.WiFi("MyWiFi-SSID", "MyWiFi-Pass", PayloadGenerator.WiFi.Authentication.WPA);
        //qrCodeData = qrGenerator.CreateQrCode(wifiPayload.ToString(), QRCodeGenerator.ECCLevel.Q);

        //        return qrCodeData;
        //    }

        public async Task<bool> CreateMailMessage(string mail)
        {
            bool success = false;
            string to = mail;
            string from = "mytdvse151417@fpt.edu.vn";
            MailMessage message = new MailMessage(from, to);

            message.Subject = "Đơn hàng của bạn đã được đặt thành công";
            message.Body = @"Vui lòng đưa mã QR này cho shipper để xác nhận đã giao hàng thành công nhé!";
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
        public async Task<OrderResponse> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                string refixOrderName = "FFPT";
                var orderCount = _unitOfWork.Repository<Order>().GetAll()
                    .Where(x => ((DateTime)x.CheckInDate).Date.Equals(DateTime.Now.Date)).Count() + 1;

                request.OrderName = refixOrderName + "-" + orderCount.ToString().PadLeft(3, '0');
                var order = _mapper.Map<CreateOrderRequest, Order>(request);

                order.OrderStatus = (int)OrderStatusEnum.Pending;

                await _unitOfWork.Repository<Order>().InsertAsync(order);
                await _unitOfWork.CommitAsync();
                try
                {
                    CreateMailMessage(request.CustomerEmail);
                    BackgroundJob.Enqueue(() => CreateMailMessage(request.CustomerEmail));
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return _mapper.Map<Order, OrderResponse>(order);
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create Order Error!!!", e.InnerException?.Message);
            }
        }
    }
}

