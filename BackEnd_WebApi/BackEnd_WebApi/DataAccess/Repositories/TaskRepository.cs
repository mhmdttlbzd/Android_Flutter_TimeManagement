using BackEnd_WebApi.DataAccess.Entities;
using BackEnd_WebApi.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BackEnd_WebApi.DataAccess.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TaskRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Creat(ApplicationTask input)
        {
            await _dbContext.Tasks.AddAsync(input);
            var res = await _dbContext.SaveChangesAsync();
            if ( res > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<List<ApplicationTask>> GetByCategoryId(int categoryId)
        {
            return await _dbContext.Tasks.Where(t => t.CategoryId == categoryId).AsNoTracking().ToListAsync();
        }
        public async Task<bool> Start(int taskId)
        {
            var isActive = _dbContext.TimeHistories.Any(t => t.ToDate == null && t.ApplicationTaskId == taskId);
            if (isActive)
            {
                return false;
            }
            await _dbContext.TimeHistories.AddAsync(new TimeHistory { ApplicationTaskId = taskId });
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> End(int taskId)
        {
            var task = await _dbContext.TimeHistories.FirstAsync(t => t.ToDate == null && t.ApplicationTaskId == taskId);
            if (task != null)
            {
                task.ToDate = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeletTimeHistory(int id)
        {
            var time = await _dbContext.TimeHistories.FirstAsync(t => t.Id == id);
            _dbContext.TimeHistories.Remove(time);
            return true;
        }
        public async Task<List<ApplicationTask>> GetTaskHistory(string userId)
        {
           return await _dbContext.Tasks.Where(t => t.UserId == userId).Include(t => t.timeHistories).AsNoTracking().ToListAsync();
        }
    }
}
