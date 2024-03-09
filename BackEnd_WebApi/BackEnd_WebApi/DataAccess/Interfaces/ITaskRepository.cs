using BackEnd_WebApi.DataAccess.Entities;

namespace BackEnd_WebApi.DataAccess.Interfaces
{
    public interface ITaskRepository
    {
        Task<bool> Creat(ApplicationTask input);
        Task<List<ApplicationTask>> GetByCategoryId(int categoryId);
        Task<bool> Start(int taskId);
        Task<bool> End(int taskId);
        Task<bool> DeletTimeHistory(int id);
        Task<List<ApplicationTask>> GetTaskHistory(string userId);
    }
}
