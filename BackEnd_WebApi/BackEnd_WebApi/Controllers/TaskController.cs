using Azure;
using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BackEnd_WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
            var categories = await _categoryService.GetAll();
            var responce = new ApiResponce<List<CategoryResponceDto>>
            {
                Succeeded = true,
                Content = categories
            };
            return Ok(responce);
        }

        [HttpGet("GetTasksByCategoryId")]
        public  async Task<IActionResult> GetTasksByCategoryId(int categoryId)
        {
            var tasks =await _taskService.GetByCategoryId(categoryId);
            var responce = new ApiResponce<List<TaskResponceDto>>
            {
                Succeeded = true,
                Content = tasks
            };
            return Ok(responce);
        }

        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask(CreateTaskDto input)
        {
            var res = await _taskService.Creat(input, User?.Identity?.Name ?? string.Empty);
            var responce = new ApiResponce();
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
            var res = await _taskService.Start(taskId);
            var responce = new ApiResponce();

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
            var res = await _taskService.End(taskId);
            var responce = new ApiResponce();

            if (res == true)
            {
                responce.Succeeded = true;
                return Ok(responce);
            }
            responce.Message = "you dont have a task in progress";
            return BadRequest(responce);
        }

        [HttpDelete("DeleteTaskHistory")]
        public async Task<IActionResult> DeleteTaskHistory(int historyId)
        {
            var res = await _taskService.DeleteTimeHistory(historyId);
            var responce = new ApiResponce();

            if (res == true)
            {
                responce.Succeeded = true;
                return Ok(responce);
            }
            responce.Message = "id is not found";
            return BadRequest(responce);
        }
        [HttpGet("GetHistory")]
        public async Task<IActionResult> GetHistory()
        {
            var res = await _taskService.GetTaskHistory(User?.Identity?.Name ?? string.Empty);
            var responce = new ApiResponce<List<TimeHistoryResponceDto>>();

            if (res != null)
            {
                responce.Succeeded = true;
                responce.Content = res;
                return Ok(responce);
            }
            responce.Message = "login again";
            return BadRequest(responce);
        }
    }
}
