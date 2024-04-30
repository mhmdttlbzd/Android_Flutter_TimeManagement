using BackEnd_WebApi.DataAccess.Entities;
using BackEnd_WebApi.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackEnd_WebApi.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public  async Task<List<Category>> GetAll(string userId)
        {
            return await _dbContext.Category.Where(c => c.UserId == userId || c.UserId == null).AsNoTracking().Include(c => c.Tasks).ToListAsync();
        }

        public async Task<bool> Create(Category category)
        {
            await _dbContext.Category.AddAsync(category);
            var res = await _dbContext.SaveChangesAsync();
            if (res != 0) return true;
            return false;
        }
        public async Task<bool> Delete(int id,string userId)
        {
            var cat = await _dbContext.Category.FirstOrDefaultAsync(x => x.Id == id);
            if (cat?.UserId != userId) throw new ApplicationException("you dont have delete this category");
            _dbContext.Category.Remove(cat);
            var res = await _dbContext.SaveChangesAsync();
            if (res != 0) return true;
            return false;
        }
        public async Task<bool> Edit(int id,string name,string userId)
        {
            var cat = await _dbContext.Category.FirstOrDefaultAsync(x => x.Id == id);
            if (cat?.UserId != userId) throw new ApplicationException("you dont have edit this category");
            if (cat != null) cat.Name = name;
            var res = await _dbContext.SaveChangesAsync();
            if (res != 0) return true;
            return false;
        }
        public async Task<bool> Clear(string userId)
        {
            var cats = await _dbContext.Category.Where(x => x.UserId == userId).ToListAsync();
            _dbContext.Category.RemoveRange(cats);
            var res = await _dbContext.SaveChangesAsync();
            if (res != 0) return true;
            return false;
        }

    }
}
