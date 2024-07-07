using BackEnd_WebApi.DataAccess.Entities;
using BackEnd_WebApi.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace BackEnd_WebApi.DataAccess.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TagRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int?> CraeteTag(string name)
        {
            var tag = new Tag { Name = name };
            await _dbContext.Tags.AddAsync(tag);
            var res = await _dbContext.SaveChangesAsync();
            if (res >0) { return tag.Id; }
            return null;
        }
        public async Task<int?> GetTagIdByName(string name)
        {
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == name);
            return tag != null ? tag.Id : null;  
        }
        public async Task<bool> AddToHistory(int tagId,int historyId)
        {
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
            if (tag != null)
            {
                var history = await _dbContext.TimeHistories.FirstOrDefaultAsync(t => t.Id == historyId);
                if (history != null)
                {
                    history.Tags.Add(tag);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
        public async Task<List<Tag>> GetAll(string username)
        {
            return await _dbContext.TimeHistories.Where(t => t.ApplicationTask.User.UserName == username ).SelectMany(t => t.Tags).ToListAsync();
        }
    }
}
