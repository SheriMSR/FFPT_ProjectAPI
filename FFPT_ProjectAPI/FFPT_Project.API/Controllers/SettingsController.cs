using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace FFPT_Project.API.Controllers
{
    [Route(Helpers.SettingVersionApi.ApiVersion)]
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;
        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        /// <summary>
        /// Get List Room
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("GetRoomList")]
        public async Task<ActionResult<List<RoomResponse>>> GetRoomList([FromQuery] RoomResponse request)
        {
            try
            {
                var rs = await _settingsService.GetRoomList(request);
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest();
            }
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
                var rs = await _settingsService.GetListTimeslot(request, paging);
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
