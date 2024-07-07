using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Interfaces;
using BackEnd_WebApi.DataAccess.Entities;
using BackEnd_WebApi.DataAccess.Interfaces;
using BackEnd_WebApi.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BackEnd_WebApi.Application.Services
{
    public class AlarmService : IAlarmService
    {
        private readonly AlarmRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AlarmService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _repository = new AlarmRepository();
        }

        public AlarmService()
        {
            _repository = new AlarmRepository();
        }
        public int HaveNotify(string userName)
        {
            return _repository.HaveNotify(userName);
        }
        private TimeSpan GetTime(string time)
        {
            try
            {
                var res = new TimeSpan();
                var split = time.Split(':');
                if (split.Length == 2) res = new TimeSpan(int.Parse(split[0]), int.Parse(split[1]), 0);
                else if (split.Length == 1) res = new TimeSpan(int.Parse(split[0]), 0, 0);
                else throw new Exception();
                return res;
            }
            catch
            {
                throw new ApplicationException("enter time in 00:00 format");
            }
        }

        private DateTime? GetDate(string date, char split)
        {

            if (string.IsNullOrEmpty(date)) return null;
            if (!Regex.IsMatch(date, @"^\d{4}" + split + @"\d{2}" + split + @"\d{2}")) throw new ApplicationException($"date must be in yyyy{split}mm{split}dd format");
            var splitDate = date.Split(split);
            var cal = new PersianCalendar();
            var res = cal.ToDateTime(int.Parse(splitDate[0]), int.Parse(splitDate[1]), int.Parse(splitDate[2]), 0, 0, 0, 0);
            return res;
        }
        public async Task SetAlarm(string date, string time, int taskId, int[] daysInWeek, string description)
        {
            string days = "";
            for (int i = 0; i < daysInWeek.Length; i++)
            {
                if (daysInWeek[i] == 0) continue;
                if (daysInWeek[i] > 7 || daysInWeek[i] < 1) throw new ApplicationException("days is incorrect");
                if (i == daysInWeek.Length - 1)
                {
                    days += daysInWeek[i];
                }
                else days += daysInWeek[i] + ",";
            }
            if (!Regex.IsMatch(time, @"^\d{2}:\d{2}")) throw new ApplicationException("enter time in 00:00 format");
            var alarm = new Alarm
            {
                Date = GetDate(date, '/'),
                Description = description ?? string.Empty,
                TaskId = taskId,
                Time = time,
                DaysInWeek = days
            };
            if (alarm.DaysInWeek == "" && alarm.Date == null) alarm.DaysInWeek = "1,2,3,4,5,6,7";
            await _repository.SetAlarm(alarm);
        }
        public List<NotifyDto> GetNotifies(string username)
        {
            var res = _repository.GetNotifies(username);
            return res;
        }
        public async Task<ApiResponce<List<AlarmDto>>> GetAlarms(string username)
        {
            var alarms = await _repository.GetAll(username);
            var dtos = new List<AlarmDto>();
            var cal = new PersianCalendar();

            foreach (var alarm in alarms)
            {
                var days = new List<int>();
                if (!string.IsNullOrEmpty(alarm.DaysInWeek))
                {
                    foreach (var i in alarm.DaysInWeek.Split(','))
                    {
                        if (!string.IsNullOrEmpty(i))
                            days.Add(int.Parse(i));
                    }
                }

                var date = alarm.Date ?? DateTime.Now;
                var mo = cal.GetMonth(date) < 10 ? "0" + cal.GetMonth(date) : "" + cal.GetMonth(date);
                var day = cal.GetDayOfMonth(date) < 10 ? "0" + cal.GetDayOfMonth(date) : "" + cal.GetDayOfMonth(date);
                dtos.Add(new AlarmDto
                {
                    Id = alarm.Id,
                    Date = cal.GetYear(date) + "/" + mo + "/" + day + " --- " + cal.GetDayOfWeek(date) + " --- " + cal.GetHour(date) + ":" + cal.GetMinute(date),
                    Description = alarm.Description,
                    TaskName = alarm.Task.Name,
                    Time = alarm.Time,
                    DaysInWeek = days
                });
            }
            var res = new ApiResponce<List<AlarmDto>>(username)
            {
                Succeeded = true,
                Content = dtos
            };
            return res;
        }
        public async Task DeleteAlarm(int id, string username)
        {
            await _repository.Delete(id, username);
        }

    }
}
