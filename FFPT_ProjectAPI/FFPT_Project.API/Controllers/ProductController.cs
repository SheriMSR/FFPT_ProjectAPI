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
        /// Get Product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProducts([FromQuery] ProductResponse request, [FromQuery] PagingRequest paging)
        {
            var rs = await _productService.GetProducts(request, paging);
            return Ok(rs);
        }

        /// <summary>
        /// Get Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductById([FromQuery] int id)
        {
            var rs = await _productService.GetProductById(id);
            return Ok(rs);
        }

        /// <summary>
        /// Get Product By Store
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpGet("GetProductByStore/{storeId}")]
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProductByStore([FromQuery] int storeId, [FromQuery] PagingRequest paging)
        {
            var rs = await _productService.GetProductByStore(storeId, paging);
            return Ok(rs);
        }

        /// <summary>
        /// Get Product By Time Slot
        /// </summary>
        [HttpGet("GetProductByTimeSlot/{timeSlotId}")]
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
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromQuery] CreateProductRequest request)
        {
            var rs = await _productService.CreateProduct(request);
            return Ok(rs);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponse>> UpdateProduct(int id,[FromQuery] UpdateProductRequest request)
        {
            var rs = await _productService.UpdateProduct(id, request);
            return Ok(rs);
        }
    }
}

