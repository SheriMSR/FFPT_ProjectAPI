using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chilkat;
using FFPT_Project.Data.Entity;
using FFPT_Project.Data.UnitOfWork;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Exceptions;
using FFPT_Project.Service.Helpers;
using IronBarCode;
using Microsoft.EntityFrameworkCore;
using Reso.Sdk.Core.Custom;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using ZXing.QrCode.Internal;
using static FFPT_Project.Service.Helpers.Enum;

namespace FFPT_Project.Service.Service
{
    public interface IOrderService
    {
        //Task<bool> CreateMailMessage(int order);
        Task<List<OrderResponse>> CreateOrder(CreateOrderRequest request);
        Task<double> CreatePreOrder(CreateOrderRequest request);
        Task<PagedResults<OrderResponse>> GetOrderByOrderStatus(OrderStatusEnum orderStatus, int customerId, PagingRequest paging);
        Task<PagedResults<OrderResponse>> GetOrders(PagingRequest paging);
        Task<OrderResponse> GetOrderById(int orderId);
        Task<OrderResponse> UpdateOrderStatus(int orderId, OrderStatusEnum orderStatus);
        Task<OrderResponse> GetToUpdateOrderStatus(int orderId);
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

        public async static Task<string> getHtml(OrderResponse order)
        {
            try
            {
                string head =
                    "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">" +
                    "<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta content=\"width=device-width, initial-scale=1\" name=\"viewport\">\r\n    <meta name=\"x-apple-disable-message-reformatting\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta content=\"telephone=no\" name=\"format-detection\">\r\n    <title></title>\r\n    <!--[if (mso 16)]>\r\n    <style type=\"text/css\">\r\n    a {text-decoration: none;}\r\n    </style>\r\n    <![endif]-->\r\n    <!--[if gte mso 9]><style>sup { font-size: 100% !important; }</style><![endif]-->\r\n    <!--[if gte mso 9]>\r\n<xml>\r\n    <o:OfficeDocumentSettings>\r\n    <o:AllowPNG></o:AllowPNG>\r\n    <o:PixelsPerInch>96</o:PixelsPerInch>\r\n    </o:OfficeDocumentSettings>\r\n</xml>\r\n<![endif]-->\r\n</head>" +
                    "<body>\r\n    <div class=\"es-wrapper-color\">\r\n        <!--[if gte mso 9]>\r\n\t\t\t<v:background xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"t\">\r\n\t\t\t\t<v:fill type=\"tile\" color=\"#ffffff\"></v:fill>\r\n\t\t\t</v:background>\r\n\t\t<![endif]-->\r\n        <table class=\"es-wrapper\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">\r\n            <tbody>\r\n                <tr>\r\n                    <td class=\"esd-email-paddings\" valign=\"top\">\r\n                        <table cellpadding=\"0\" cellspacing=\"0\" class=\"es-content esd-footer-popover\" align=\"center\">\r\n                            <tbody>\r\n                                <tr>\r\n                                    <td class=\"esd-stripe\" align=\"center\" bgcolor=\"#BEE7F9\" style=\"background-color: #bee7f9;\">\r\n                                        <table class=\"es-content-body\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\" style=\"background-color: transparent;\">\r\n                                            <tbody>\r\n                                                <tr class=\"es-mobile-hidden\">\r\n                                                    <td class=\"esd-structure es-p20t es-p20r es-p20l\" align=\"left\">\r\n                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                            <tbody>\r\n                                                                <tr>\r\n                                                                    <td width=\"560\" class=\"esd-container-frame\" align=\"center\" valign=\"top\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td align=\"center\" class=\"esd-block-spacer\" height=\"40\"></td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                </tr>\r\n                                                            </tbody>\r\n                                                        </table>\r\n                                                    </td>\r\n                                                </tr>\r\n                                                <tr>\r\n                                                    <td class=\"esd-structure\" align=\"left\">\r\n                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                            <tbody>\r\n                                                                <tr>\r\n                                                                    <td width=\"600\" class=\"esd-container-frame\" align=\"center\" valign=\"top\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td align=\"center\" class=\"esd-block-image\" style=\"font-size: 0px;\"><a target=\"_blank\" href=\"https://viewstripo.email\"><img class=\"adapt-img\" src=\"https://ahymal.stripocdn.email/content/guids/CABINET_e04c1fedd65f56fd365d4f9fd2632863/images/packaging_stamps.jpg\" alt style=\"display: block;\" width=\"600\"></a></td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                </tr>\r\n                                                            </tbody>\r\n                                                        </table>\r\n                                                    </td>\r\n                                                </tr>" +
                    $"            <tr>\r\n                <td class=\"esd-structure es-p30t es-p20b es-p20r es-p20l\" align=\"left\" bgcolor=\"#ffffff\" style=\"background-color: #ffffff;\">\r\n                    <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td width=\"560\" class=\"esd-container-frame\" align=\"center\" valign=\"top\">\r\n                                    <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                        <tbody>\r\n                                            <tr>\r\n                                                <td align=\"center\" class=\"esd-block-text\">\r\n                                                    <h1>Đơn hàng&nbsp;{order.OrderName} đã được shipper tiếp nhận</h1>\r\n                                                </td>\r\n                                            </tr>\r\n                                            <tr>\r\n                                                <td align=\"center\" class=\"esd-block-text\">\r\n                                                    <p style=\"font-family: arial, 'helvetica neue', helvetica, sans-serif; color: #368e81;\">Vui lòng đưa mã QR này cho shipper để xác nhận đã giao hàng thành công nhé!</p>\r\n                                                </td>\r\n                                            </tr>\r\n                                            <tr>\r\n                                                                                                           <tr>\r\n                                                                                    <td align=\"center\" class=\"esd-block-image\" style=\"font-size: 0px;\"><a target=\"_blank\"><img class=\"adapt-img\" src=cid:MyPic alt style=\"display: block;\" width=\"269\"></a></td>\r\n                                                                                </tr>                                    </tbody>\r\n                                    </table>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                </td>\r\n            </tr>" +
                    $" <tr>\r\n                                                    <td class=\"esd-structure es-p20\" align=\"left\" bgcolor=\"#ffffff\" style=\"background-color: #ffffff;\">\r\n                                                        <!--[if mso]><table width=\"560\" cellpadding=\"0\" cellspacing=\"0\"><tr><td width=\"270\" valign=\"top\"><![endif]-->\r\n                                                        <table cellpadding=\"0\" cellspacing=\"0\" class=\"es-left\" align=\"left\">\r\n                                                            <tbody>\r\n                                                                <tr>\r\n                                                                    <td width=\"270\" class=\"esd-container-frame es-m-p20b\" align=\"left\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" bgcolor=\"#ffffff\" style=\"background-color: #ffffff;\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td align=\"left\" class=\"esd-block-text es-m-txt-l es-p10t\">\r\n                                                                                        <h2>Delivered To</h2>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                                <tr>\r\n                                                                                    <td align=\"left\" class=\"esd-block-spacer es-p10t es-p10b es-m-txt-l\" style=\"font-size:0\">\r\n                                                                                        <table border=\"0\" width=\"50%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"display: inline-table; width: 50% !important;\">\r\n                                                                                            <tbody>\r\n                                                                                                <tr>\r\n                                                                                                    <td style=\"border-bottom: 5px solid #ff8e72; background: none; height: 1px; width: 100%; margin: 0px;\"></td>\r\n                                                                                                </tr>\r\n                                                                                            </tbody>\r\n                                                                                        </table>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                </tr>\r\n                                                            </tbody>\r\n                                                        </table>\r\n                                                        <!--[if mso]></td><td width=\"20\"></td><td width=\"270\" valign=\"top\"><![endif]-->\r\n                                                        <table cellpadding=\"0\" cellspacing=\"0\" class=\"es-right\" align=\"right\">\r\n                                                            <tbody>\r\n                                                                <tr>\r\n                                                                    <td width=\"270\" class=\"esd-container-frame\" align=\"left\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" bgcolor=\"#ffffff\" style=\"background-color: #ffffff;\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td align=\"left\" class=\"esd-block-text es-p10t\">\r\n                                                                                        <p><strong>{order.CustomerInfo.Name}</strong></p>\r\n                                                                                        <p><strong>Room</strong>: {order.RoomNumber}<br><strong>Phone</strong>: {order.CustomerInfo.Phone}</p>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                </tr>\r\n                                                            </tbody>\r\n                                                        </table>\r\n                                                        <!--[if mso]></td></tr></table><![endif]-->\r\n                                                    </td>\r\n                                                </tr>" +
                    "<tr>\r\n                                                    <td class=\"esd-structure es-p20t es-p20r es-p20l\" align=\"left\" bgcolor=\"#ffffff\" style=\"background-color: #ffffff;\">\r\n                                                        <!--[if mso]><table width=\"560\" cellpadding=\"0\" cellspacing=\"0\"><tr><td width=\"270\" valign=\"top\"><![endif]-->\r\n                                                        <table cellpadding=\"0\" cellspacing=\"0\" align=\"left\" class=\"es-left\">\r\n                                                            <tbody>\r\n                                                                <tr>\r\n                                                                    <td width=\"270\" class=\"esd-container-frame es-m-p20b\" align=\"center\" valign=\"top\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td align=\"left\" class=\"esd-block-text es-m-txt-l es-p10t\">\r\n                                                                                        <h2>Ordered Details</h2>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                                <tr>\r\n                                                                                    <td align=\"left\" class=\"esd-block-spacer es-p10t es-p10b es-m-txt-l\" style=\"font-size:0\">\r\n                                                                                        <table border=\"0\" width=\"50%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"display: inline-table; width: 50% !important;\">\r\n                                                                                            <tbody>\r\n                                                                                                <tr>\r\n                                                                                                    <td style=\"border-bottom: 5px solid #ff8e72; background: none; height: 1px; width: 100%; margin: 0px;\"></td>\r\n                                                                                                </tr>\r\n                                                                                            </tbody>\r\n                                                                                        </table>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                </tr>\r\n                                                            </tbody>\r\n                                                        </table>\r\n                                                        <!--[if mso]></td><td width=\"20\"></td><td width=\"270\" valign=\"top\"><![endif]-->\r\n                                                        <table cellpadding=\"0\" cellspacing=\"0\" class=\"es-right\" align=\"right\">\r\n                                                            <tbody>\r\n                                                                <tr class=\"es-mobile-hidden\">\r\n                                                                    <td width=\"270\" align=\"left\" class=\"esd-container-frame\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td align=\"center\" class=\"esd-block-spacer\" height=\"40\"></td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                </tr>\r\n                                                            </tbody>\r\n                                                        </table>\r\n                                                        <!--[if mso]></td></tr></table><![endif]-->\r\n                                                    </td>\r\n                                                </tr><td align=\"center\" class=\"esd-block-spacer\" height=\"10\">\r\n</td>"
                    ;
                string content = "";
                foreach (var orderDetail in order.OrderDetails)
                {
                    content += $"<tr>\r\n                                                    <td class=\"esd-structure es-p5t es-p5b es-p20r es-p20l esdev-adapt-off\" align=\"left\" bgcolor=\"#ffffff\" style=\"background-color: #ffffff;\" esdev-config=\"h1\">\r\n                                                        <table width=\"560\" cellpadding=\"0\" cellspacing=\"0\" class=\"esdev-mso-table\">\r\n                                                            <tbody>\r\n                                                                <tr>\r\n                                                                    <td class=\"esdev-mso-td\" valign=\"top\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" class=\"es-left\" align=\"left\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td width=\"30\" class=\"es-m-p0r esd-container-frame\" align=\"center\">\r\n                                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                                                            <tbody>\r\n                                                                                                <tr>\r\n                                                                                                    <td align=\"left\" class=\"esd-block-text\">\r\n                                                                                                        <p>{orderDetail.Quantity}</p>\r\n                                                                                                    </td>\r\n                                                                                                </tr>\r\n                                                                                            </tbody>\r\n                                                                                        </table>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                    <td width=\"20\"></td>\r\n                                                                    <td class=\"esdev-mso-td\" valign=\"top\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" class=\"es-left\" align=\"left\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td width=\"30\" class=\"esd-container-frame\" align=\"center\">\r\n                                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                                                            <tbody>\r\n                                                                                                <tr>\r\n                                                                                                    <td align=\"center\" class=\"esd-block-image\" style=\"font-size: 0px;\"><a target=\"_blank\" href=\"https://viewstripo.email\"><img src=\"{orderDetail.Image}\" alt style=\"display: block;\" class=\"p_image\" width=\"30\"></a></td>\r\n                                                                                                </tr>\r\n                                                                                            </tbody>\r\n                                                                                        </table>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                    <td width=\"20\"></td>\r\n                                                                    <td class=\"esdev-mso-td\" valign=\"top\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" class=\"es-left\" align=\"left\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td width=\"315\" align=\"center\" class=\"esd-container-frame\">\r\n                                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                                                            <tbody>\r\n                                                                                                <tr>\r\n                                                                                                    <td align=\"left\" class=\"esd-block-text\">\r\n                                                                                                        <p class=\"p_name\">{orderDetail.ProductName}</p>\r\n                                                                                                    </td>\r\n                                                                                                </tr>\r\n                                                                                            </tbody>\r\n                                                                                        </table>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                    <td width=\"20\"></td>\r\n                                                                    <td class=\"esdev-mso-td\" valign=\"top\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" class=\"es-right\" align=\"right\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td width=\"125\" align=\"left\" class=\"esd-container-frame\">\r\n                                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                                                            <tbody>\r\n                                                                                                <tr>\r\n                                                                                                    <td align=\"right\" class=\"esd-block-text\">\r\n                                                                                                        <p class=\"p_price\">{orderDetail.FinalAmount}</p>\r\n                                                                                                    </td>\r\n                                                                                                </tr>\r\n                                                                                            </tbody>\r\n                                                                                        </table>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                </tr>\r\n                                                            </tbody>\r\n                                                        </table>\r\n                                                    </td>\r\n                                                </tr><tr>\r\n                                                    <td class=\"esd-structure es-p20r es-p20l\" align=\"left\" bgcolor=\"#ffffff\" style=\"background-color: #ffffff;\">\r\n                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                            <tbody>\r\n                                                                <tr>\r\n                                                                    <td width=\"560\" class=\"esd-container-frame\" align=\"center\" valign=\"top\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td align=\"center\" class=\"esd-block-spacer es-p10t es-p10b\" style=\"font-size:0\">\r\n                                                                                        <table border=\"0\" width=\"100%\" height=\"100%\" cellpadding=\"0\" cellspacing=\"0\">\r\n                                                                                            <tbody>\r\n                                                                                                <tr>\r\n                                                                                                    <td style=\"border-bottom: 1px solid #cccccc; background: none; height: 1px; width: 100%; margin: 0px;\"></td>\r\n                                                                                                </tr>\r\n                                                                                            </tbody>\r\n                                                                                        </table>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                </tr>\r\n                                                            </tbody>\r\n                                                        </table>\r\n                                                    </td>\r\n                                                </tr>";
                }
                string amount = $"<tr>\r\n                                                    <td class=\"esd-structure es-p5t es-p40b es-p20r es-p20l esdev-adapt-off\" align=\"left\" bgcolor=\"#ffffff\" style=\"background-color: #ffffff;\">\r\n                                                        <table width=\"560\" cellpadding=\"0\" cellspacing=\"0\" class=\"esdev-mso-table\">\r\n                                                            <tbody>\r\n                                                                <tr>\r\n                                                                    <td class=\"esdev-mso-td\" valign=\"top\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" class=\"es-left\" align=\"left\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td width=\"270\" class=\"es-m-p0r esd-container-frame\" align=\"center\">\r\n                                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                                                            <tbody>\r\n                                                                                                <tr>\r\n                                                                                                    <td align=\"left\" class=\"esd-block-text es-m-txt-l\">\r\n                                                                                                        <h2>Total Cost</h2>\r\n                                                                                                    </td>\r\n                                                                                                </tr>\r\n                                                                                            </tbody>\r\n                                                                                        </table>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                    <td width=\"20\"></td>\r\n                                                                    <td class=\"esdev-mso-td\" valign=\"top\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" class=\"es-right\" align=\"right\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td width=\"270\" align=\"left\" class=\"esd-container-frame\">\r\n                                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                                                            <tbody>\r\n                                                                                                <tr>\r\n                                                                                                    <td align=\"right\" class=\"esd-block-text es-m-txt-r\">\r\n                                                                                                        <h2>{order.FinalAmount} VND</h2>\r\n                                                                                                    </td>\r\n                                                                                                </tr>\r\n                                                                                            </tbody>\r\n                                                                                        </table>\r\n                                                                                    </td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                </tr>\r\n                                                            </tbody>\r\n                                                        </table>\r\n                                                    </td>\r\n                                                </tr>";
                string foot = "<tr class=\"es-mobile-hidden\">\r\n                                                    <td class=\"esd-structure es-p20t es-p20r es-p20l\" align=\"left\">\r\n                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                            <tbody>\r\n                                                                <tr>\r\n                                                                    <td width=\"560\" class=\"esd-container-frame\" align=\"center\" valign=\"top\">\r\n                                                                        <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                                                            <tbody>\r\n                                                                                <tr>\r\n                                                                                    <td align=\"center\" class=\"esd-block-spacer\" height=\"0\"></td>\r\n                                                                                </tr>\r\n                                                                            </tbody>\r\n                                                                        </table>\r\n                                                                    </td>\r\n                                                                </tr>\r\n                                                            </tbody>\r\n                                                        </table>\r\n                                                    </td>\r\n                                                </tr>\r\n                                            </tbody>\r\n                                        </table>\r\n                                    </td>\r\n                                </tr>\r\n                            </tbody>\r\n                        </table>\r\n                    </td>\r\n                </tr>\r\n            </tbody>\r\n        </table>\r\n    </div>\r\n</body>\r\n\r\n</html>";
                string message = head + content + amount + foot;
                return message; // return HTML Table as string from this function  
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> CreateMailMessage(OrderResponse order)
        {
            GeneratedBarcode Qrcode = IronBarCode.QRCodeWriter.CreateQrCode($"https://ffptprojectapi20221008031045.azurewebsites.net/api/v1.0/order/OrderFinish?orderId={order.Id}");
            Qrcode.AddAnnotationTextAboveBarcode("Scan me o((>ω< ))o");
            Qrcode.SaveAsPng("MyBarCode.png");

            var path = Path.Combine(Environment.CurrentDirectory + @"\MyBarCode.png");
            LinkedResource LinkedImage = new LinkedResource(path);
            LinkedImage.ContentId = "MyPic";
            var mess = await getHtml(order);
            bool success = false;
            string to = order.CustomerInfo.Email;
            string from = "mytdvse151417@fpt.edu.vn";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Đơn hàng đây, đơn hàng đây! FFood mang niềm vui đến cho bạn nè!!!!";
            message.Body = mess;

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mess, null, "text/html");
            htmlView.LinkedResources.Add(LinkedImage);
            message.AlternateViews.Add(htmlView);

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
        public async Task<OrderResponse> GetToUpdateOrderStatus(int orderId)
        {
            try
            {
                var order = await _unitOfWork.Repository<Order>().GetAll()
                            .Where(x => x.Id == orderId)
                            .FirstOrDefaultAsync();
                order.OrderStatus = (int)OrderStatusEnum.Finish;

                await _unitOfWork.Repository<Order>().UpdateDetached(order);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<OrderResponse>(order);
            }
            catch (Exception e)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Error", e.Message);
            }
        }
        public async Task<List<OrderResponse>> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                List<OrderResponse> listOrderResult = new List<OrderResponse>();
                #region checkDeliveryPhone
                var check = CheckVNPhoneEmail(request.DeliveryPhone);
                if (check)
                {
                    if (!request.DeliveryPhone.StartsWith("+84"))
                    {
                        request.DeliveryPhone = request.DeliveryPhone.TrimStart(new char[] { '0' });
                        request.DeliveryPhone = "+84" + request.DeliveryPhone;
                    }
                }
                else
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Wrong Phone", request.DeliveryPhone.ToString());
                }
                #endregion

                //Phân có bao nhiêu store trong order detail
                HashSet<int> listStore = new HashSet<int>();
                foreach (var detail in request.OrderDetails)
                {
                    listStore.Add(detail.SupplierStoreId);
                }
                var feeShipping = 0;
                // tách đơn 
                foreach (var storeId in listStore)
                {
                    Order order = new Order();

                    #region OrderName
                    string refixOrderName = "FFPT";
                    var orderCount = _unitOfWork.Repository<Order>().GetAll()
                        .Where(x => ((DateTime)x.CheckInDate).Date.Equals(DateTime.Now.Date)).Count() + 1;

                    order.OrderName = refixOrderName + "-" + orderCount.ToString().PadLeft(3, '0');
                    #endregion

                    order.CheckInDate = DateTime.Now;

                    #region order delivery shipping fee
                    if (request.OrderType == (int)OrderTypeEnum.Delivery)
                    {
                        //ShippingFee
                        if (feeShipping == 0)
                        {
                            feeShipping = 5000;
                        }
                        else
                        {
                            feeShipping = 2000;
                        }
                    }

                    #endregion
                    order.ShippingFee = feeShipping;
                    order.OrderStatus = (int)OrderStatusEnum.Pending;
                    order.OrderType = request.OrderType;

                    order.TimeSlotId = request.TimeSlotId;
                    order.DeliveryPhone = request.DeliveryPhone;

                    order.CustomerId = request.CustomerId;
                    order.Customer = _unitOfWork.Repository<Customer>().Find(x => x.Id == request.CustomerId);

                    order.RoomId = request.RoomId;
                    order.Room = _unitOfWork.Repository<Room>().Find(x => x.Id == request.RoomId);

                    #region Order detail
                    List<OrderDetail> listOrderDetail = new List<OrderDetail>();
                    List<OrderDetailResponse> listOrderDetailResponse = new List<OrderDetailResponse>();
                    foreach (var detail in request.OrderDetails)
                    {
                        order.TotalAmount = 0;
                        if (detail.SupplierStoreId == storeId)
                        {
                            order.TotalAmount += (double)detail.FinalAmount;
                            var product = _unitOfWork.Repository<ProductInMenu>().GetAll()
                                .Include(x => x.Product)
                                .Where(x => x.Id == detail.ProductInMenuId)
                                .FirstOrDefault();

                            OrderDetail orderDetail = new OrderDetail();

                            orderDetail.ProductName = product.Product.Name;
                            orderDetail.Quantity = detail.Quantity;
                            orderDetail.FinalAmount = (double)detail.FinalAmount;
                            orderDetail.Status = product.Product.Status;
                            orderDetail.ProductInMenuId = product.Id;

                            listOrderDetail.Add(orderDetail);

                            var orderDetailResult = _mapper.Map<OrderDetail, OrderDetailResponse>(orderDetail);
                            orderDetailResult.Image = product.Product.Image;
                            listOrderDetailResponse.Add(orderDetailResult);
                        }
                    }
                    #endregion
                    order.FinalAmount = (double)(order.TotalAmount + order.ShippingFee);
                    order.OrderDetails = listOrderDetail;

                    await _unitOfWork.Repository<Order>().InsertAsync(order);
                    await _unitOfWork.CommitAsync();

                    var storeResult = _unitOfWork.Repository<Store>().Find(x => x.Id == storeId);

                    var customerResult = _mapper.Map<Customer, CustomerResponse>(order.Customer);

                    var orderResult = _unitOfWork.Repository<Order>().GetAll()
                        .Include(x => x.OrderDetails)
                        .Where(x => x.OrderName == order.OrderName && x.CheckInDate == order.CheckInDate)
                        .Select(x => new OrderResponse()
                        {
                            Id = x.Id,
                            OrderName = x.OrderName,
                            CheckInDate = x.CheckInDate,
                            TotalAmount = x.TotalAmount,
                            ShippingFee = x.ShippingFee,
                            FinalAmount = x.FinalAmount,
                            OrderStatus = x.OrderStatus,
                            DeliveryPhone = x.DeliveryPhone,
                            OrderType = x.OrderType,
                            TimeSlotId = x.TimeSlotId,
                            RoomId = (int)x.RoomId,
                            RoomNumber = x.Room.RoomNumber,
                            SupplierStoreId = storeId,
                            StoreName = storeResult.Name,
                            CustomerInfo = customerResult,
                            OrderDetails = listOrderDetailResponse
                        })
                        .FirstOrDefault();

                    listOrderResult.Add(orderResult);
                    CreateMailMessage(orderResult);

                }
                return listOrderResult;

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

        public async Task<double> CreatePreOrder(CreateOrderRequest request)
        {
            try
            {
                var feeShipping = 0;
                //Phân có bao nhiêu store trong order detail
                HashSet<int> listStore = new HashSet<int>();
                foreach (var detail in request.OrderDetails)
                {
                    listStore.Add(detail.SupplierStoreId);
                }

                // tách đơn 
                foreach (var storeId in listStore)
                {
                    if (request.OrderType == (int)OrderTypeEnum.Delivery)
                    {
                        //ShippingFee
                        if (feeShipping == 0)
                        {
                            feeShipping = 5000;
                        }
                        else
                        {
                            feeShipping += 2000;
                        }
                    }
                }
                return feeShipping;
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

        public async Task<PagedResults<OrderResponse>> GetOrders(PagingRequest paging)
        {
            try
            {
                var orderList = _unitOfWork.Repository<Order>().GetAll()
                                 .ToList();

                List<OrderResponse> result = new List<OrderResponse>();
                foreach (var order in orderList)
                {
                    var product = order.OrderDetails.FirstOrDefault().ProductInMenu.Product;
                    var store = _unitOfWork.Repository<Store>().Find(x => x.Id == product.SupplierStoreId);
                    var orderDetail = _mapper.Map<List<OrderDetailResponse>>(order.OrderDetails);
                    var customerResult = _mapper.Map<Customer, CustomerResponse>(order.Customer);
                    var orderResult = new OrderResponse()
                    {
                        Id = order.Id,
                        OrderName = order.OrderName,
                        CheckInDate = order.CheckInDate,
                        TotalAmount = order.TotalAmount,
                        ShippingFee = order.ShippingFee,
                        FinalAmount = order.FinalAmount,
                        OrderStatus = order.OrderStatus,
                        DeliveryPhone = order.DeliveryPhone,
                        OrderType = order.OrderType,
                        TimeSlotId = order.TimeSlotId,
                        RoomId = (int)order.RoomId,
                        RoomNumber = order.Room.RoomNumber,
                        SupplierStoreId = store.Id,
                        StoreName = store.Name,
                        CustomerInfo = customerResult,
                        OrderDetails = orderDetail
                    };
                    result.Add(orderResult);
                }
                return PageHelper<OrderResponse>.Paging(result, paging.Page, paging.PageSize);
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Error", ex.Message);
            }
        }
        public async Task<PagedResults<OrderResponse>> GetOrderByOrderStatus(OrderStatusEnum orderStatus, int customerId, PagingRequest paging)
        {
            try
            {
                var orderList = _unitOfWork.Repository<Order>().GetAll()
                            .Where(x => x.OrderStatus == (int)orderStatus
                            && x.Customer.Id == customerId)
                            .ToList();

                List<OrderResponse> result = new List<OrderResponse>();
                foreach (var order in orderList)
                {
                    var product = order.OrderDetails.FirstOrDefault().ProductInMenu.Product;
                    var store = _unitOfWork.Repository<Store>().Find(x => x.Id == product.SupplierStoreId);
                    var orderDetail = _mapper.Map<List<OrderDetailResponse>>(order.OrderDetails);
                    var customerResult = _mapper.Map<Customer, CustomerResponse>(order.Customer);
                    var orderResult = new OrderResponse()
                    {
                        Id = order.Id,
                        OrderName = order.OrderName,
                        CheckInDate = order.CheckInDate,
                        TotalAmount = order.TotalAmount,
                        ShippingFee = order.ShippingFee,
                        FinalAmount = order.FinalAmount,
                        OrderStatus = order.OrderStatus,
                        DeliveryPhone = order.DeliveryPhone,
                        OrderType = order.OrderType,
                        TimeSlotId = order.TimeSlotId,
                        RoomId = (int)order.RoomId,
                        RoomNumber = order.Room.RoomNumber,
                        SupplierStoreId = store.Id,
                        StoreName = store.Name,
                        CustomerInfo = customerResult,
                        OrderDetails = orderDetail
                    };
                    result.Add(orderResult);
                }
                return PageHelper<OrderResponse>.Paging(result, paging.Page, paging.PageSize);
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

        public async Task<OrderResponse> GetOrderById(int orderId)
        {
            try
            {
                var order = _unitOfWork.Repository<Order>().Find(x => x.Id == orderId);

                if (order == null)
                    throw new CrudException(HttpStatusCode.NotFound, "Order Id not found", orderId.ToString());
                var customerResult = _mapper.Map<Customer, CustomerResponse>(order.Customer);
                var orderResult = _mapper.Map<List<OrderDetailResponse>>(order.OrderDetails);

                var storeId = order.OrderDetails.FirstOrDefault().ProductInMenu.Product.SupplierStoreId;
                var storeName = order.OrderDetails.FirstOrDefault().ProductInMenu.Product.SupplierStore.Name;

                return new OrderResponse()
                {
                    Id = order.Id,
                    OrderName = order.OrderName,
                    CheckInDate = order.CheckInDate,
                    TotalAmount = order.TotalAmount,
                    ShippingFee = order.ShippingFee,
                    FinalAmount = order.FinalAmount,
                    OrderStatus = order.OrderStatus,
                    DeliveryPhone = order.DeliveryPhone,
                    OrderType = order.OrderType,
                    TimeSlotId = order.TimeSlotId,
                    RoomId = (int)order.RoomId,
                    RoomNumber = order.Room.RoomNumber,
                    SupplierStoreId = storeId,
                    StoreName = storeName,
                    CustomerInfo = customerResult,
                    OrderDetails = orderResult
                };
            }
            catch (CrudException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Error", ex.Message);
            }

        }
    }
}

