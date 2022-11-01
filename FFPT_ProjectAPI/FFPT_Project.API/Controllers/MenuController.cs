using FFPT_Project.Data.Entity;
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
        /// Get List Menu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetListMenu")]
        public async Task<ActionResult<PagedResults<MenuResponse>>> GetListMenu([FromQuery] MenuResponse request, [FromQuery] PagingRequest paging)
        {
            try
            {
                var rs = await _menuService.GetListMenu(request, paging);
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Get Menu By Id
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpGet("GetMenuById")]
        public async Task<ActionResult<PagedResults<MenuResponse>>> GetMenuById([FromQuery] int menuId)
        {
            try
            {
                var rs = await _menuService.GetMenuById(menuId);
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get Menu By Time Slot
        /// </summary>
        /// <param name="timeSlotId"></param>
        /// <returns></returns>
        [HttpGet("GetMenuByTimeSlot")]
        public async Task<ActionResult<MenuResponse>> GetMenuByTimeSlot([FromQuery] int timeSlotId, [FromQuery] PagingRequest paging)
        {
            try
            {
                var rs = await _menuService.GetMenuByTimeSlot(timeSlotId, paging);
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Create Menu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("CreateMenu")]
        public async Task<ActionResult<MenuResponse>> CreateMenu([FromBody] CreateMenuRequest request)
        {
            try
            {
                var rs = await _menuService.CreateMenu(request);
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Update Menu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateMenu")]
        public async Task<ActionResult<MenuResponse>> UpdateMenu([FromQuery] int menuId, [FromBody] UpdateMenuRequest request)
        {
            try
            {
                var rs = await _menuService.UpdateMenu(menuId, request);
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }   

    }
}
