using BackEnd_WebApi.DataAccess.Entities;

namespace BackEnd_WebApi.DataAccess.Interfaces
{
    public interface ITagRepository
    {
        Task<bool> AddToHistory(int tagId, int historyId);
        Task<int?> GetTagIdByName(string name);
        Task<int?> CraeteTag(string name);
        Task<List<Tag>> GetAll(string userId);
    }
}
