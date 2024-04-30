using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd_WebApi.Controllers
{

    [Route("[controller]")]
    public class ValidationController : ControllerBase
    {
        private readonly IUserService _userService;
        public ValidationController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterInputDto dto)
        {
            var token = await _userService.Register(dto);
            var res = new ApiResponce<RegisterResponce>()
            {
                Succeeded = true,
                Content = token,
                Message = "Save the forgotPassword for when you forget your password"
            };
            return Ok(res);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _userService.Login(dto);
            var res = new ApiResponce<RegisterResponce>()
            {
                Succeeded = true,
                Content = token
            };
            return Ok(res);
        }

        [HttpPatch("RefreshToken")]
        public async Task<IActionResult> RefreshToken(AuthenticatedResponse input)
        {

            var res = new ApiResponce<string>()
            {
                Succeeded = true,
                Content = await _userService.RefreshToken(input)
            };
            return Ok(res);
        }

        [Authorize]
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string newPassword,string corrntPassword)
        {
            var res = new ApiResponce();
            if (!ModelState.IsValid)
            {
                res.Message = ModelState.First().Value.ToString();
            }
           
            else if (await _userService.ResetPassword(User?.Identity?.Name ?? string.Empty, newPassword, corrntPassword))
            {
                res.Succeeded = true;
            }
            return Ok(res);
        }

        [HttpPut("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto input)
        {
            await _userService.ForgetPassword(input);
            var res = new ApiResponce
            {
                Succeeded = true
            };
            return Ok(res);
        }

        [HttpPut("GetForgetPassword")]
        public async Task<IActionResult> GetForgetPassword(string userName)
        {
            var pass = await _userService.GetForgetPasswpord(userName);
            var res = new ApiResponce<string>
            {
                Content = pass,
                Succeeded = true
            };
            return Ok(res);
        }
        
    }
}
