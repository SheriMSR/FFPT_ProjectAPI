using FFPT_Project.Data.Entity;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Dynamic.Core;
using System.Net.NetworkInformation;

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
            try
            {
                var rs = await _productInMenuService.GetProductInMenu(request, paging);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Product In Menu By Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetProductInMenuById")]
        public async Task<ActionResult<ProductInMenuResponse>> GetProductInMenuById([FromQuery] int Id)
        {
            try
            {
                var rs = await _productInMenuService.GetProductInMenuById(Id);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Product In Menu By Store
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpGet("GetProductInMenuByStore")]
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProductInMenuByStore([FromQuery] int storeId, [FromQuery] PagingRequest paging)
        {
            try
            {
                var rs = await _productInMenuService.GetProductInMenuByStore(storeId, paging);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Product In Menu By Menu
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpGet("GetProductInMenuByMenu")]
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProductInMenuByMenu([FromQuery] int menuId, [FromQuery] PagingRequest paging)
        {
            try
            {
                var rs = await _productInMenuService.GetProductInMenuByMenu(menuId, paging);
                return Ok(rs);
            }
            catch (Exception ex )
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Product In Menu By Time Slot
        /// </summary>
        /// <param name="timeSlotId"></param>
        /// <returns></returns>
        [HttpGet("GetProductInMenuByTimeSlot")]
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProductInMenuByTimeSlot([FromQuery] int timeSlotId, [FromQuery] PagingRequest paging)
        {
            try
            {
                var rs = await _productInMenuService.GetProductInMenuByTimeSlot(timeSlotId, paging);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                var rs = await _productInMenuService.SearchProductInMenu(searchString, timeSlotId, paging);
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Check Product
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        [HttpGet("CheckProduct")]
        public async Task<ActionResult<bool>> CheckProductInMenu([FromQuery] string productCode)
        {
            try
            {
                var rs = await _productInMenuService.CheckProductInMenu(productCode);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Product Menu By Category
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        [HttpGet("GetProductByCategory")]
        public async Task<ActionResult<PagedResults<ProductInMenuResponse>>> GetProductInMenuByCategory([FromQuery] int cateId, [FromQuery] int timeSlotId, [FromQuery] PagingRequest paging)
        {
            try
            {
                var rs = await _productInMenuService.GetProductInMenuByCategory(cateId, timeSlotId, paging);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create Product Menu
        /// </summary>
        [HttpPost("CreateProductInMenu")]
        public async Task<ActionResult<List<ProductInMenuResponse>>> CreateProductInMenu([FromBody] CreateProductInMenuRequest request)
        {
            try
            {
                var rs = await _productInMenuService.CreateProductInMenu(request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Product Menu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateProductInMenu")]
        public async Task<ActionResult<ProductInMenuResponse>> UpdateProductInMenu([FromQuery] int productInMenuId, [FromBody] UpdateProductInMenuRequest request)
        {
            try
            {
                var rs = await _productInMenuService.UpdateProductInMenu(productInMenuId, request);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Product Menu
        /// </summary>
        /// <param name="productInMenuId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProductInMenu")]
        public async Task<ActionResult<ProductInMenuResponse>> DeleteProductInMenu([FromQuery] int productInMenuId)
        {
            try
            {
                var rs = await _productInMenuService.DeleteProductInMenu(productInMenuId);
                return Ok(rs);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
