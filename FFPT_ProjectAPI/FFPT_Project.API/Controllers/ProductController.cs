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
        public async Task<ActionResult<PagedResults<ProductResponse>>> GetProducts([FromQuery] ProductRequest request, [FromQuery] PagingRequest paging)
        {
            var rs = await _productService.GetProducts(request, paging);
            return Ok(rs);
        }
    }
}
