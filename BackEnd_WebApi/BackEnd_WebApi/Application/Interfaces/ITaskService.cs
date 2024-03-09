using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.DataAccess.Entities;

namespace BackEnd_WebApi.Application.Interfaces
{
    public interface ITaskService
    {
        Task<bool> Creat(CreateTaskDto input, string userName);
        Task<List<TaskResponceDto>> GetByCategoryId(int categoryId);
        Task<bool> Start(int taskId);
        Task<bool> End(int taskId);
        Task<bool> DeleteTimeHistory(int id);
        Task<List<TimeHistoryResponceDto>> GetTaskHistory(string userId);
    }
}
