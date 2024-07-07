using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd_WebApi.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AlarmController : ControllerBase
    {
        private readonly IAlarmService _alarmService;
        public AlarmController(IAlarmService alarmService)
        {
            _alarmService = alarmService;
        }
        [HttpGet("GetNotifies")]
        public IActionResult GetNotifies()
        {
            var notifies = _alarmService.GetNotifies(User.Identity.Name);
            var res = new BaseApiResponce<List<NotifyDto>>()
            {
                Content = notifies,
                Succeeded = true
            };
            return Ok(res);
        }
        [HttpGet("GetAlarms")]
        public async Task<IActionResult> GetAlarms()
        {
            return Ok(await _alarmService.GetAlarms(User.Identity.Name));
        }
        [HttpDelete("DeleteAlarm")]
        public async Task<IActionResult> DeleteAlarm(int id)
        {
            await _alarmService.DeleteAlarm(id, User.Identity.Name);
            return Ok(new ApiResponce(User.Identity.Name) { Succeeded = true });
        }

        [HttpPost("SetAlarm")]
        public async Task<IActionResult> SetAlarm(string date, string time, int taskId, string daysInWeek, string description)
        {
            if (string.IsNullOrEmpty(time)) throw new ApplicationException("time is required");
           
            int[] l;
            if (!string.IsNullOrEmpty(daysInWeek))
            {
                l = new int[daysInWeek.Split(',').Length];
                var split = daysInWeek.Split(',');
                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i] != "")
                        l[i] = int.Parse(split[i]);
                }
            }
            else l = new int[0];
            await _alarmService.SetAlarm(date, time, taskId, l, description);
            return Ok(new ApiResponce(User?.Identity?.Name ?? string.Empty) { Succeeded = true });
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAlarm(string date, string time, int[] daysInWeek, string descroption)
        {
            return Ok();
        }
    }
}
