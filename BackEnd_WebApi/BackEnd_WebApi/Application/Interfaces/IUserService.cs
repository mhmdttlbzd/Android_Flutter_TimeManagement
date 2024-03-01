using BackEnd_WebApi.Application.Dtos;

namespace BackEnd_WebApi.Application.Interfaces
{
    public interface IUserService 
    {
        Task<string> Login(LoginDto dto);
        Task<string> Register(RegisterInputDto inputDto);
    }
}
