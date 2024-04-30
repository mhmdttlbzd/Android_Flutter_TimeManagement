using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.DataAccess.Entities;

namespace BackEnd_WebApi.Application.Interfaces
{
    public interface ITaskService
    {
        Task<bool> Creat(CreateTaskDto input, string userName);
        Task<List<GeneralResponceDto>> GetByCategoryId(int categoryId, string username);
        Task<bool> Start(int taskId,string username);
        Task<bool> End(int taskId, string username);
        Task<GeneralResponceDto> GetById(int id);
        Task<ApiResponce> Delete(int id, string username);
        Task<ApiResponce> Clear(string userId);
        Task<ApiResponce> Edit(int id, string name, string userId);
    }
}
