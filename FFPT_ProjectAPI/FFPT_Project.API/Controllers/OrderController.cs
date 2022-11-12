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

        ///// <summary>
        ///// Send QR to mail
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <returns></returns>
        //[HttpPost("SendQRToMail")]
        //public async Task<ActionResult> CreateMailMessage(int orderId)
        //{
        //    try
        //    {
        //        var result = _orderService.CreateMailMessage(orderId);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        /// <summary>
        /// Get Orders
        /// </summary>
        [HttpGet]
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
        /// Get Order By Id
        /// </summary>
        [HttpGet("{Id}")]
        public async Task<ActionResult<PagedResults<OrderResponse>>> GetOrderById([FromQuery] int Id)
        {
            try
            {
                var rs = await _orderService.GetOrderById(Id);
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
        [HttpPut("UpdateOrderStatus")]
        public async Task<ActionResult<OrderResponse>> UpdateOrderStatus([FromQuery] int orderId, [FromQuery] OrderStatusEnum orderStatus)
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
