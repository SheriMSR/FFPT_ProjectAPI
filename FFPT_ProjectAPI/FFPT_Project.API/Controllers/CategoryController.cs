using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace FFPT_Project.API.Controllers
{
    [Route(Helpers.SettingVersionApi.ApiVersion)]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        /// <summary>
        /// Get List Category
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpGet("GetListCategory")]
        public async Task<ActionResult<PagedResults<CategoryResponse>>> GetListCategory([FromQuery] PagingRequest paging)
        {
            try
            {
                var rs = await _categoryService.GetListCategory(paging);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
