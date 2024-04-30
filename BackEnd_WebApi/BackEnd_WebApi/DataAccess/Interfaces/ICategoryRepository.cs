using BackEnd_WebApi.DataAccess.Entities;

namespace BackEnd_WebApi.DataAccess.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAll(string userId);
        Task<bool> Create(Category category);
        Task<bool> Delete(int id,string userId);
        Task<bool> Edit(int id, string name,string userId);
        Task<bool> Clear(string userId);
    }
}
