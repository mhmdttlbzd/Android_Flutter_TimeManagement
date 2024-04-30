using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Exeptions;
using BackEnd_WebApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Identity.Client;

namespace BackEnd_WebApi.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpDelete("DeleteTaskHistory")]
        public async Task<IActionResult> DeleteTaskHistory(int historyId)
        {
            var res = await _historyService.DeleteTimeHistory(historyId,User.Identity.Name);
            var responce = new ApiResponce();

            if (res == true)
            {
                responce.Succeeded = true;
                return Ok(responce);
            }
            responce.Message = "id is not found";
            return BadRequest(responce);
        }

        [HttpGet("GetAllHistories")]
        public async Task<IActionResult> GetAllHistories()
        {
            var res = await _historyService.GetTaskHistory(User?.Identity?.Name ?? string.Empty);
            var responce = new ApiResponce<List<TimeHistoryResponceDto>>();

            if (res != null)
            {
                responce.Succeeded = true;
                responce.Content = res;
                if (res.Count()!= 0) responce.Message = $"{GetSubTime(res)} \n {GetBusiestTime(res)}";
                return Ok(responce);
            }
            responce.Message = "login again";
            return BadRequest(responce);
        }

        private string GetSubTime(List<TimeHistoryResponceDto> input)
        {
            int hour = 0;
            int minute = 0;
            int number = 0;
            foreach (var history in input)
            {
                var l = history.Time.Split(':');
                hour += int.Parse(l[0]);
                minute += int.Parse(l[1]);
                number++;
            }
            int avg = (hour * 60 + minute)/number;
            int avgHour = avg / 60;
            int avgMinute = avg % 60;
            hour += minute / 60;
            minute %= 60;
            return $"time spent : {hour} hours and {minute} minutes \n avrage time spent per day : {avgHour} hours and {avgMinute} minutes";
        }

        private string GetBusiestTime(List<TimeHistoryResponceDto> input)
        {
            int number = 0;
            int fromTime = 0;
            int toTime = 0;
            foreach (var history in input)
            {
                var lf = history.FromTime.Split(':');
                var lt = history.ToTime.Split(':');
                fromTime += int.Parse(lf[0]) * 60 + int.Parse(lf[1]);
                toTime += int.Parse(lt[0]) * 60 + int.Parse(lt[1]);
                number++;
            }
            fromTime /= number; toTime /= number;
            string res = "busiest time of day : "+ fromTime /60 +":"+ fromTime%60 +" to "+ toTime/60 +":"+toTime%60;
            return res;
        }


        [HttpGet("GetHistories")]
        public async Task<IActionResult> GetHistories(int tagId = -1, int categoryId = -1,string fromDate = "",string toDate = "")
        {
            var res = await _historyService.GetTaskHistory(User?.Identity?.Name ?? string.Empty, categoryId, tagId,fromDate,toDate);
            var responce = new ApiResponce<List<TimeHistoryResponceDto>>();

            if (res != null)
            {
                responce.Succeeded = true;
                responce.Content = res;
                if (res.Count != 0) responce.Message =$"{ GetSubTime(res)} \n{GetBusiestTime(res)}";
                return Ok(responce);
            }
            responce.Message = "login again";
            return BadRequest(responce);
        }

        [HttpPut("AddTag")]
        public async Task<IActionResult> AddTag(string name, int historyId)
        {
            bool isSucceed = await _historyService.AddTag(name, historyId);
            var res = new ApiResponce()
            {
                Succeeded = isSucceed
            };
            if (!isSucceed) { throw new IncorrectInputExaception(); }
            return Ok(res);
        }

        [HttpGet("GetAllTags")]
        public async Task<IActionResult> GetAllTags()
        {
            var res = await _historyService.GetAllTags();
            var responce = new ApiResponce<List<GeneralResponceDto>>();

            if (res != null)
            {
                responce.Succeeded = true;
                responce.Content = res;
                return Ok(responce);
            }
            responce.Message = "login again";
            return BadRequest(responce);

        }
        [HttpDelete("ClearMyHistory")]
        public async Task<IActionResult> ClearMyHistory()
        {
            var res = await _historyService.Clear(User.Identity.Name);
            return Ok(res);
        }
        [HttpPost("CreateFakeHistory")]
        public async Task<IActionResult> CreateFakeHistory(string date, string fromTime, string toTime, int taskId)
        {
            await _historyService.CreateFakeHistory(date, fromTime, toTime, taskId);
            return Ok(new ApiResponce { Message ="",Succeeded = true});
        }
    }
}
