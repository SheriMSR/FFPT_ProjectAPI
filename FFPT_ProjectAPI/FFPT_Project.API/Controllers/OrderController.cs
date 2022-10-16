using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace FFPT_Project.API.Controllers
{
    [Route(Helpers.SettingVersionApi.ApiVersion)]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<OrderResponse>> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                var result = await _orderService.CreateOrder(request);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message)
;            }
        }

        /// <summary>
        /// Send QR to mail
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        [HttpPost("SendQRToMail")]
        public async Task<ActionResult> CreateMailMessage(string mail)
        {
            try
            {
                var result = await _orderService.CreateMailMessage(mail);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
