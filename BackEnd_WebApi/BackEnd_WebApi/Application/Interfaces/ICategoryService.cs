using BackEnd_WebApi.Application.Dtos;

namespace BackEnd_WebApi.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponceDto>> GetAll();
    }
}
