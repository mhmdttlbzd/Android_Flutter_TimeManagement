using BackEnd_WebApi.DataAccess.Entities;

namespace BackEnd_WebApi.DataAccess.Interfaces
{
    public interface ITaskHistoryRepository
    {
        Task<bool> DeletTimeHistory(int id,string userId);
        Task<List<ApplicationTask>> GetTaskHistory(string userId);
        Task<bool> Clear(string userId);
        Task Create(TimeHistory history);
    }
}
