using BackEnd_WebApi.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd_WebApi.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class HomeController : ControllerBase
    {
        [HttpPost("Index")]
        public IActionResult Index()
        {
            
            return Ok(User?.Identity?.Name);
        }
    }
}
