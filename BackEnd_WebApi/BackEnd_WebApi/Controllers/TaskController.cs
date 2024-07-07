using Azure;
using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Interfaces;
using BackEnd_WebApi.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BackEnd_WebApi.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        public TaskController(ITaskService taskService, ICategoryService categoryService)
        {
            _taskService = taskService;
            _categoryService = categoryService;
        }
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAll(User.Identity.Name);
            var responce = new ApiResponce<List<CategoryResponceDto>>(User.Identity.Name)
            {
                Succeeded = true,
                Content = categories
            };
            return Ok(responce);
        }

        [HttpGet("GetTasksByCategoryId")]
        public  async Task<IActionResult> GetTasksByCategoryId(int categoryId)
        {
            var tasks =await _taskService.GetByCategoryId(categoryId,User?.Identity?.Name ?? string.Empty);
            if (tasks.Count() == 0) { tasks.Add(new GeneralResponceDto { Id = -1, Name = "" }); }
            var responce = new ApiResponce<List<GeneralResponceDto>>(User.Identity.Name)
            {
                Succeeded = true,
                Content = tasks
            };
            return Ok(responce);
        }
        [HttpGet("GetAll")]
        public  async Task<IActionResult> GetAll()
        {
            var tasks =await _taskService.GetAll(User?.Identity?.Name ?? string.Empty);
            if (tasks.Count() == 0) { tasks.Add(new GeneralResponceDto { Id = -1, Name = "" }); }
            var responce = new ApiResponce<List<GeneralResponceDto>>(User.Identity.Name)
            {
                Succeeded = true,
                Content = tasks
            };
            return Ok(responce);
        }


        

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var task =await _taskService.GetById(id);
            var res = new ApiResponce<GeneralResponceDto>(User.Identity.Name)
            {
                Content = task,
                Succeeded = true
            };
            return Ok(res);
        }

        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask(CreateTaskDto input)
        {
            var res = await _taskService.Creat(input, User?.Identity?.Name ?? string.Empty);
            var responce = new ApiResponce(User.Identity.Name);
            if (res == true)
            {
                responce.Succeeded = true;
                return Ok(responce);
            }
            responce.Message = "incorrect inputs";
            return BadRequest(responce);
        }

        [HttpPut("Start")]
        public async Task<IActionResult> Start(int taskId)
        {
            var res = await _taskService.Start(taskId,User.Identity.Name);
            var responce = new ApiResponce(User.Identity.Name);

            if (res == true)
            {
                responce.Succeeded = true;
                return Ok(responce);
            }
            responce.Message = "Task is running";
            return BadRequest(responce);
        }

        [HttpPut("End")]
        public async Task<IActionResult> End(int taskId)
        {
            var res = await _taskService.End(taskId,User.Identity.Name);
            var responce = new ApiResponce(User.Identity.Name);

            if (res == true)
            {
                responce.Succeeded = true;
                return Ok(responce);
            }
            responce.Message = "you dont have a task in progress";
            return BadRequest(responce);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _taskService.Delete(id, User?.Identity?.Name ?? string.Empty);
            if (res.Succeeded) return Ok(res);
            return BadRequest(res);
        }  
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(int id,string name)
        {
            var res = await _taskService.Edit(id,name, User?.Identity?.Name ?? string.Empty);
            if (res.Succeeded) return Ok(res);
            return BadRequest(res);
        } 
        [HttpDelete("Clear")]
        public async Task<IActionResult> Clear()
        {
            var res = await _taskService.Clear( User?.Identity?.Name ?? string.Empty);
            if (res.Succeeded) return Ok(res);
            return BadRequest(res);
        } 
        [HttpDelete("ClearCategories")]
        public async Task<IActionResult> ClearCategories()
        {
            var res = await _categoryService.ClearWithTasks( User?.Identity?.Name ?? string.Empty);
            if (res.Succeeded) return Ok(res);
            return BadRequest(res);
        }  
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var res = await _categoryService.Delete(id, User?.Identity?.Name ?? string.Empty);
            if (res.Succeeded) return Ok(res);
            return BadRequest(res);
        }
        [HttpPut("EditCategory")]
        public async Task<IActionResult> EditCategory(int id,string name)
        {
            var res = await _categoryService.Edit(id,name, User?.Identity?.Name ?? string.Empty);
            if (res.Succeeded) return Ok(res);
            return BadRequest(res);
        }  
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(string name)
        {
            var res = await _categoryService.Create(name, User?.Identity?.Name ?? string.Empty);
            if (res.Succeeded) return Ok(res);
            return BadRequest(res);
        }
        [HttpGet("GetFriendlyTasks")]
        public async Task<IActionResult> GetFriendlyTasks()
        {
            return Ok(new ApiResponce<List<GeneralResponceDto>>(User.Identity.Name) { Succeeded = true, Content = await _taskService.GetFriendlyTasks(User.Identity.Name) }) ;
        }
        [HttpPut("ShareToFriend")]
        public async Task<IActionResult> ShareToFriend(int taskId,string friendUsername)
        {
            await _taskService.ShareToFriend(User.Identity.Name,taskId,friendUsername);
            return Ok(new ApiResponce(User.Identity.Name) { Succeeded = true});
        }
    }
}
