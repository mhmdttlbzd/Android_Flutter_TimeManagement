using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Interfaces;
using BackEnd_WebApi.DataAccess.Entities;
using BackEnd_WebApi.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace BackEnd_WebApi.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskService(ITaskRepository taskRepository,UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _taskRepository = taskRepository;
        }
        public async Task<bool> Creat(CreateTaskDto input,string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var task = new ApplicationTask()
                {
                    Name = input.Name,
                    CategoryId = input.CategoryId,
                    UserId = user.Id
                };
                return await _taskRepository.Creat(task);
            }
            return false;
        }

        public async Task<List<TaskResponceDto>> GetByCategoryId(int categoryId)
        {
            var tasks = await _taskRepository.GetByCategoryId(categoryId);
            var res = new List<TaskResponceDto>();
            foreach (var task in tasks)
            {
                res.Add(new TaskResponceDto
                {
                    Name = task.Name,
                    Id = task.Id
                });
            }
            return res;

        }

        public async Task<bool> Start(int taskId)
        {
            return await _taskRepository.Start(taskId);
        }

        public async Task<bool> End(int taskId) => await _taskRepository.End(taskId);
        public async Task<bool> DeleteTimeHistory(int id) => await _taskRepository.DeletTimeHistory(id);
        public async Task<List<TimeHistoryResponceDto>> GetTaskHistory(string userName) 
        {
            var user = await _userManager.FindByNameAsync(userName);
            var res = new List<TimeHistoryResponceDto>();
            var cal = new PersianCalendar();
           var tasks =await _taskRepository.GetTaskHistory(user?.Id ?? string.Empty);
            foreach (var task  in tasks)
            {
                foreach (var h in task.timeHistories)
                {
                    var t = h.ToDate ?? DateTime.Now;
                    var time = t - h.FromDate;
                    var r = new TimeHistoryResponceDto
                    {
                        Date = cal.GetYear(h.FromDate) + "/" + cal.GetMonth(h.FromDate) + "/" + cal.GetDayOfMonth(h.FromDate) + " --- " + cal.GetDayOfWeek(h.FromDate),
                        FromTime = cal.GetHour(h.FromDate) + ":" + cal.GetMinute(h.FromDate),
                        ToTime = cal.GetHour(h.ToDate ?? DateTime.Now) + ":" + cal.GetMinute(h.ToDate ?? DateTime.Now),
                        Time = time.Hours + ":" + time.Minutes,
                        TaskName = task.Name,
                       Id= h.Id
                    };
                    res.Add(r);
                }
            }
            return res;
        }
    }
}
