using FFPT_Project.Data.Entity;
using FFPT_Project.Service.DTO.Request;
using FFPT_Project.Service.DTO.Response;
using FFPT_Project.Service.Service;
using Google.Apis.Auth;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FFPT_Project.API.Controllers
{
    [Route(Helpers.SettingVersionApi.ApiVersion)]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Get Customer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PagedResults<CustomerResponse>>> GetCustomers([FromQuery] CustomerResponse request, [FromQuery] PagingRequest paging)
        {
            var rs = await _customerService.GetCustomers(request, paging);
            return Ok(rs);
        }



        /// <summary>
        /// Login
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<CustomerResponse>> Login([FromBody] ExternalAuthRequest data)
        {
            var result = await _customerService.Login(data);          
            return Ok(result);
        }
    }
}
