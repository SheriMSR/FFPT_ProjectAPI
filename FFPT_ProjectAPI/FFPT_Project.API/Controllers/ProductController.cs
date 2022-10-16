using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace FFPT_Project.API.Controllers
{
    [Route(Helpers.SettingVersionApi.ApiVersion)]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductServices _productService;

        public ProductController(IProductServices productServices)
        {
            _productService = productServices;
        }

        /// <summary>
        /// Get List Product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProducts([FromQuery] ProductResponse request, [FromQuery] PagingRequest paging)
        {
            try
            {
                var rs = await _productService.GetProducts(request, paging);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Get Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById")]
        public async Task<ActionResult<ProductResponse>> GetProductById([FromQuery] int id)
        {
            var rs = await _productService.GetProductById(id);
            return Ok(rs);
        }

        /// <summary>
        /// Get Product By Store
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("GetProductByCode")]
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProductByCode([FromQuery] string code)
        {
            var rs = await _productService.GetProductByCode(code);
            return Ok(rs);
        }

        /// <summary>
        /// Get Product By Store
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpGet("GetProductByStore")]
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProductByStore([FromQuery] int storeId, [FromQuery] PagingRequest paging)
        {
            var rs = await _productService.GetProductByStore(storeId, paging);
            return Ok(rs);
        }

        /// <summary>
        /// Get Product By Category
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        [HttpGet("GetProductByCategory")]
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProductByCategory([FromQuery] int cateId, [FromQuery] PagingRequest paging)
        {
            var rs = await _productService.GetProductByCategory(cateId, paging);
            return Ok(rs);
        }

        /// <summary>
        /// Get Product By Time Slot
        /// </summary>
        [HttpGet("GetProductByTimeSlot")]
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProductByTimeSlot([FromQuery] int timeSlotId, [FromQuery] PagingRequest paging)
        {
            var rs = await _productService.GetProductByTimeSlot(timeSlotId, paging);
            return Ok(rs);
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
        {
            var rs = await _productService.CreateProduct(request);
            return Ok(rs);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<ProductResponse>> UpdateProduct([FromQuery] int productId,[FromBody] UpdateProductRequest request)
        {
            var rs = await _productService.UpdateProduct(productId, request);
            return Ok(rs);
        }

        /// <summary>
        /// Search Product
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="timeSlotId"></param>
        /// <returns></returns>
        [HttpGet("SearchProductInMenu")]
        public async Task<ActionResult<PagedResults<ProductResponse>>> SearchProductInMenu([FromQuery] string searchString, [FromQuery] int timeSlotId, [FromQuery] PagingRequest paging)
        {
            var rs = await _productService.SearchProductInMenu(searchString, timeSlotId, paging);
            return Ok(rs);
        }


        /// <summary>
        /// Search Product
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="timeSlotId"></param>
        /// <returns></returns>
        [HttpGet("SearchProduct")]
        public async Task<ActionResult<PagedResults<ProductResponse>>> SearchProduct([FromQuery] string searchString, [FromQuery] int timeSlotId, [FromQuery] PagingRequest paging)
        {
            var rs = await _productService.SearchProduct(searchString, timeSlotId, paging);
            return Ok(rs);
        }
    }
}

