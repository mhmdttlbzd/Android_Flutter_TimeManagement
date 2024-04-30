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

        public async Task<List<GeneralResponceDto>> GetByCategoryId(int categoryId,string username)
        {
            var tasks = await _taskRepository.GetByCategoryId(categoryId);
            var res = new List<GeneralResponceDto>();
            var user =await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                tasks = tasks.Where(x => x.UserId == user.Id).ToList();
                foreach (var task in tasks.Where(t =>  t.UserId == user.Id))
                {
                    res.Add(new GeneralResponceDto
                    {
                        Name = task.Name,
                        Id = task.Id
                    });
                }
            }
            return res;

        }
        public async Task<GeneralResponceDto> GetById(int id)
        {
            var res = new GeneralResponceDto();
            var task = await _taskRepository.GetById(id);
            if (task!= null)
            {
                res.Id = task.Id;
                res.Name = task.Name;
            }
            return res;
        }

        public async Task<bool> Start(int taskId,string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _taskRepository.Start(taskId,user.Id);
        }

        public async Task<bool> End(int taskId,string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return await _taskRepository.End(taskId,user.Id);
        }

        public async Task<ApiResponce> Delete(int id , string username)
        {

            var res = new ApiResponce();
            var user =await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var suc = _taskRepository.Delete(id,user.Id.ToString());
                res.Succeeded = suc;
                return res;
            }
            res.Succeeded = false;
            res.Message = "not foound";
            return res;
        }
        public async Task<ApiResponce> Clear(string userName)
        {
            var user =await _userManager.FindByNameAsync(userName);
            await _taskRepository.Clear(user?.Id.ToString() ?? string.Empty);
            return new ApiResponce { Succeeded = true, Message = "" };
        }
        public async Task<ApiResponce> Edit(int id, string name, string userName)
        {
            var user =await _userManager.FindByNameAsync(userName);
            _taskRepository.Edit(id, name, user?.Id.ToString() ?? string.Empty);
            return new ApiResponce { Succeeded = true, Message = "" };
        }

    }
}
