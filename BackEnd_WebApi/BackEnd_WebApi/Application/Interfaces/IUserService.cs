﻿using BackEnd_WebApi.Application.Dtos;

namespace BackEnd_WebApi.Application.Interfaces
{
    public interface IUserService 
    {
        Task<LoginResponce> Login(LoginDto dto);
        Task<RegisterResponce> Register(RegisterInputDto inputDto);
        Task<string> RefreshToken(AuthenticatedResponse input);
        Task<bool> ResetPassword(string userName, string newPassword, string oldPassword);
        Task<bool> ForgetPassword(ForgetPasswordDto input);
    }
}
