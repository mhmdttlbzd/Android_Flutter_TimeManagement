﻿using BackEnd_WebApi.Application.Exeptions;
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
            if (res > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<ApplicationTask?> GetById(int id)
        {
            return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<ApplicationTask>> GetByCategoryId(int categoryId)
        {
            return await _dbContext.Tasks.Where(t => t.CategoryId == categoryId).AsNoTracking().ToListAsync();
        }
        public async Task<bool> Start(int taskId,string username)
        {
            var isActive = _dbContext.TimeHistories.Any(t => t.ToDate == null && t.ApplicationTaskId == taskId);
            if (isActive)
            {
                return false;
            }
            var task =await _dbContext.Tasks.Include(x => x.User).FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null) throw new ApplicationException("This history is not exist");
            if (task.User.UserName != username && !task.Usernames.Split(',').ToList().Any(x => x == username)) throw new ApplicationException("you cant start this history");
            await _dbContext.TimeHistories.AddAsync(new TimeHistory { ApplicationTaskId = taskId });
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> End(int taskId,string username)
        {
            var history = await _dbContext.TimeHistories.FirstAsync(t => t.ToDate == null && t.ApplicationTaskId == taskId);
            var task = await _dbContext.Tasks.Include(x => x.User).FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null) throw new ApplicationException("This task is not exist");
            if (task.User.UserName != username && !task.Usernames.Split(',').ToList().Any(x => x == username)) throw new ApplicationException("you cant start this task");
            if (history != null)
            {
                history.ToDate = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public bool Delete(int id, string userId)
        {
            var task = _dbContext.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                throw new ApplicationException("not found history");
            }
            if (task.UserId != userId)
            {
                throw new ApplicationException("you dont have permission to delete this history");

            }
            _dbContext.Tasks.Remove(task);
            _dbContext.SaveChanges();

            return true;
        }
        public bool Edit(int id, string name, string userId)
        {
            var task = _dbContext.Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                throw new ApplicationException("not found history");
            }
            if (task.UserId != userId)
            {
                throw new ApplicationException("you dont have permission to edit this history");

            }
            task.Name = name;
            _dbContext.SaveChanges();
            return true;
        }
        public async Task<bool> Clear(string userId)
        {
            var tasks = await _dbContext.Tasks.Where(x => x.UserId == userId).ToListAsync();
            _dbContext.Tasks.RemoveRange(tasks);
            var res = await _dbContext.SaveChangesAsync();
            if (res != 0) return true;
            return false;
        }

        public async Task<List<ApplicationTask>> GetAll()
        {
            return await _dbContext.Tasks.AsNoTracking().ToListAsync();
        }

        public async Task ShareToFriend(string username, int taskId,string friendUsername)
        {
            var task = await _dbContext.Tasks.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == taskId);
            if (task == null) { throw new ApplicationException("task not found"); }
            if (task.User.Name != username) { throw new ApplicationException("not permission"); }
            if (!string.IsNullOrEmpty(task.Usernames))
            {

                task.Usernames +=","+ friendUsername;
            }
            else
            {
                if(task.Usernames == null)
                {
                    task.Usernames = "";
                }
                task.Usernames += friendUsername;
            }
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<ApplicationTask>> GetFriendlyTasks(string username)
        {
            var tasks = await _dbContext.Tasks.Where(x => x.Usernames.Contains(username)).ToListAsync();
            foreach (var task in tasks)
            {
                if (!task.Usernames.Split(',').Any(x => x == username)) tasks.Remove(task);
            }
            return tasks;
        }

    }
}
