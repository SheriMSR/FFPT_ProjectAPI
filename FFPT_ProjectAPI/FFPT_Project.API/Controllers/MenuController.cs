using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace FFPT_Project.API.Controllers
{
    [Route(Helpers.SettingVersionApi.ApiVersion)]
    [ApiController]
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// Get List Timeslot
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetListTimeslot")]
        public async Task<ActionResult<PagedResults<TimeslotResponse>>> GetListTimeslot([FromQuery] TimeslotResponse request, [FromQuery] PagingRequest paging)
        {
            try
            {
                var rs = await _menuService.GetListTimeslot(request, paging);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
