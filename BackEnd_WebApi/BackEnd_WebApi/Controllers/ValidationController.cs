﻿using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd_WebApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [AllowAnonymous]
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
            var res = new ApiResponce()
            {
                Succeeded = true,
                Content = token
            };
            return Ok(res);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _userService.Login(dto);
            var res = new ApiResponce()
            {
                Succeeded = true,
                Content = token
            };
            return Ok(res);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(AuthenticatedResponse input)
        {

            var res = new ApiResponce()
            {
                Succeeded = true,
                Content = await _userService.RefreshToken(input)
            };
            return Ok(res);
        }
    }
}
