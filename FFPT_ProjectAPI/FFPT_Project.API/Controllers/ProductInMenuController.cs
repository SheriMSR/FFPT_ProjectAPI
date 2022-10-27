using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Dynamic.Core;

namespace FFPT_Project.API.Controllers
{
    [Route(Helpers.SettingVersionApi.ApiVersion)]
    [ApiController]
    public class ProductInMenuController : Controller
    {
        private readonly IProductInMenuService _productInMenuService;

        public ProductInMenuController(IProductInMenuService productInMenuService)
        {
            _productInMenuService = productInMenuService;
        }

        /// <summary>
        /// Get List Product In Menu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedResults<ProductInMenuResponse>>> GetProductInMenu([FromQuery] ProductInMenuResponse request, [FromQuery] PagingRequest paging)
        {
            var result = await _productInMenuService.GetProductInMenu(request, paging);
            return Ok(result);
        }

        /// <summary>
        /// Get List Product In Menu By Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetProductInMenuById")]
        public async Task<ActionResult<ProductInMenuResponse>> GetProductInMenuById([FromQuery] int Id)
        {
            var result = await _productInMenuService.GetProductInMenuById(Id);
            return Ok(result);
        }

        /// <summary>
        /// Get Product In Menu By Store
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpGet("GetProductInMenuByStore")]
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProductInMenuByStore([FromQuery] int storeId, [FromQuery] PagingRequest paging)
        {
            var rs = await _productInMenuService.GetProductInMenuByStore(storeId, paging);
            return Ok(rs);
        }

        /// <summary>
        /// Get Product In Menu By Time Slot
        /// </summary>
        /// <param name="timeSlotId"></param>
        /// <returns></returns>
        [HttpGet("GetProductInMenuByTimeSlot")]
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProductInMenuByTimeSlot([FromQuery] int timeSlotId, [FromQuery] PagingRequest paging)
        {
            var rs = await _productInMenuService.GetProductInMenuByTimeSlot(timeSlotId, paging);
            return Ok(rs);
        }

        /// <summary>
        /// Search Product
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="timeSlotId"></param>
        /// <returns></returns>
        [HttpGet("SearchProduct")]
        public async Task<ActionResult<PagedResults<ProductInMenuResponse>>> SearchProductInMenu([FromQuery] string searchString, [FromQuery] int timeSlotId, [FromQuery] PagingRequest paging)
        {
            var rs = await _productInMenuService.SearchProductInMenu(searchString, timeSlotId, paging);
            return Ok(rs);
        }

        /// <summary>
        /// Get Product Menu By Category
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        [HttpGet("GetProductByCategory")]
        public async Task<ActionResult<PagedResults<ProductInMenuResponse>>> GetProductInMenuByCategory([FromQuery] int cateId, [FromQuery] PagingRequest paging)
        {
            var rs = await _productInMenuService.GetProductInMenuByCategory(cateId, paging);
            return Ok(rs);
        }

        /// <summary>
        /// Create Product Menu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("CreateProductInMenu")]
        public async Task<StatusCodeResult> CreateProductInMenu([FromBody] CreateProductInMenuRequest request)
        {
            var rs = await _productInMenuService.CreateProductInMenu(request);
            return rs;
        }

        /// <summary>
        /// Update Product Menu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateProductInMenu")]
        public async Task<StatusCodeResult> UpdateProductInMenu([FromQuery] int productInMenuId, [FromBody] UpdateProductInMenuRequest request)
        {
            var rs = await _productInMenuService.UpdateProductInMenu(productInMenuId, request);
            return rs;
        }

        /// <summary>
        /// Delete Product Menu
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProductInMenu")]
        public async Task<StatusCodeResult> DeleteProductInMenu([FromQuery] int productInMenuId)
        {
            var rs = await _productInMenuService.DeleteProductInMenu(productInMenuId);
            return rs;
        }
    }
}
