using BackEnd_WebApi.DataAccess.Entities;

namespace BackEnd_WebApi.DataAccess.Interfaces
{
    public interface ITaskRepository
    {
        Task<bool> Creat(ApplicationTask input);
        Task<List<ApplicationTask>> GetByCategoryId(int categoryId);
        Task<ApplicationTask?> GetById(int id);
        Task<bool> Start(int taskId,string username);
        Task<bool> End(int taskId, string usename);
        bool Delete(int id, string userId);
        Task<bool> Clear(string userId);
        bool Edit(int id, string name, string userId);
        Task<List<ApplicationTask>> GetAll();
        Task ShareToFriend(string username, int taskId, string friendUsername);
        Task<List<ApplicationTask>> GetFriendlyTasks(string username);
    } 
}
