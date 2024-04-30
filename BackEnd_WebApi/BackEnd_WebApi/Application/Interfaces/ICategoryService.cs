using BackEnd_WebApi.Application.Dtos;

namespace BackEnd_WebApi.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponceDto>> GetAll(string userName);
        Task<ApiResponce> Clear(string userName);
        Task<ApiResponce> ClearWithTasks(string userName);
        Task<ApiResponce> Delete(int id, string userName);
        Task<ApiResponce> Create(string name, string userName);
        Task<ApiResponce> Edit(int id, string name, string userName);
    }
}
