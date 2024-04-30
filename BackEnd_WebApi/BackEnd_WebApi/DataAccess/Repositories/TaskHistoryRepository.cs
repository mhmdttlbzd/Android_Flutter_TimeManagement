using BackEnd_WebApi.DataAccess.Entities;
using BackEnd_WebApi.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd_WebApi.DataAccess.Repositories
{
    public class TaskHistoryRepository : ITaskHistoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskHistoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeletTimeHistory(int id ,string userId)
        {
            var time = await _dbContext.TimeHistories.Include(x => x.ApplicationTask).FirstAsync(t => t.Id == id);
            if (time == null) { throw new ApplicationException("not found history"); }
            if (time.ApplicationTask.UserId != userId) { throw new ApplicationException("you dont can delet this history"); }
            _dbContext.TimeHistories.Remove(time);
            await _dbContext.SaveChangesAsync();
            return true;
        }   
        public async Task<bool> Clear(string userId)
        {
            var times = await _dbContext.TimeHistories.Where(t => t.ApplicationTask.UserId == userId).ToListAsync();
            _dbContext.TimeHistories.RemoveRange(times);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<ApplicationTask>> GetTaskHistory(string userId)
        {
            return await _dbContext.Tasks.Where(t => t.UserId == userId).Include(t => t.timeHistories)
                .ThenInclude(t => t.Tags).AsNoTracking().ToListAsync();
        }
        public async Task Create(TimeHistory history)
        {
            await _dbContext.TimeHistories.AddAsync(history);
            await _dbContext.SaveChangesAsync();
        }
    }
}
