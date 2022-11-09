using Azure.Core;
using FFPT_Project.Data.Entity;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Service;
using Microsoft.AspNetCore.Mvc;
using static FFPT_Project.Service.Helpers.Enum;

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
        public async Task<ActionResult<List<OrderResponse>>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var result = await _orderService.CreateOrder(request);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message)
;           }
        }

        /// <summary>
        /// Send QR to mail
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="orderName"></param>
        /// <returns></returns>
        //[HttpPost("SendQRToMail")]
        //public async Task<ActionResult> CreateMailMessage(string mail, string orderName)
        //{
        //    try
        //    {
        //        var result = await _orderService.CreateMailMessage(mail, orderName);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        /// <summary>
        /// Get List Orders By Order Status
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpGet("GetListOrderByOrderStatus")]
        public async Task<ActionResult<PagedResults<OrderResponse>>> GetOrderByOrderStatus([FromQuery]OrderStatusEnum orderStatus, [FromQuery] int customerId, [FromQuery] PagingRequest paging)
        {
            try
            {
                var rs = await _orderService.GetOrderByOrderStatus(orderStatus, customerId, paging);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get List Orders
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpGet("GetListOrders")]
        public async Task<ActionResult<PagedResults<OrderResponse>>> GetOrders([FromQuery] PagingRequest paging)
        {
            try
            {
                var rs = await _orderService.GetOrders(paging);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update Order 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        [HttpGet("UpdateOrderStatus")]
        public async Task<ActionResult<OrderResponse>> UpdateOrderStatus([FromQuery] int orderId, [FromBody] OrderStatusEnum orderStatus)
        {
            try
            {
                var rs = await _orderService.UpdateOrderStatus(orderId, orderStatus);
                return Ok(rs);
    }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
}
        }
    }
}
