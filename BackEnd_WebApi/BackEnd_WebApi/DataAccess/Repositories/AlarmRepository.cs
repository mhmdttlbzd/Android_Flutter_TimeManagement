using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.DataAccess.Entities;
using BackEnd_WebApi.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BackEnd_WebApi.DataAccess.Repositories
{
    public class AlarmRepository
    {
        protected readonly ApplicationDbContext _dbContext;

        public AlarmRepository()
        {
            _dbContext = new ApplicationDbContext();
        }

        public int HaveNotify(string username)
        {
            return GetNotifies(username).Count();
        }

        public List<NotifyDto> GetNotifies(string username)
        {
            var res = new List<NotifyDto>();
            var l = _dbContext.Tasks.Where(t => t.User.UserName == username).SelectMany(t => t.Alarms).Include(x => x.Task).ToList();
            var todayAlarms = new List<Alarm>();
            foreach(var a in l)
            {
                if (a.Date != null)
                {
                    if (DateTime.Now.Year == a.Date?.Year && DateTime.Now.Month == a.Date?.Month && DateTime.Now.Day == a.Date?.Day)
                    {
                        todayAlarms.Add(a);
                        continue;
                    }
                }
              
                if ((a.DaysInWeek ?? string.Empty).Split(',').ToList().Any(x => x == (DateTime.Now.DayOfWeek).ToString())) todayAlarms.Add(a);
            }
            var notifies = new List<Alarm>();
            foreach (var a in todayAlarms)
            {
                var alarmMinute = (int.Parse(a.Time.Split(':')[0]) * 60) +(int.Parse(a.Time.Split(':')[1]));
                var nowMinute = (DateTime.Now.Hour * 60) +DateTime.Now.Minute;
                if (alarmMinute - nowMinute <= 5 && alarmMinute - nowMinute >0) notifies.Add(a);
            }
            foreach (var alarm in notifies)
            {
                res.Add(new NotifyDto
                {
                    Description = alarm.Description,
                    TaskName = alarm.Task.Name,
                    TaskId = alarm.TaskId,
                    MinuteLeft = int.Parse(alarm.Time.Split(':')[0]) * 60 + int.Parse(alarm.Time.Split(':')[1]) - DateTime.Now.Hour * 60 + DateTime.Now.Minute
                });
            }
            return res;
        }
        public async Task Delete(int id, string username)
        {
            var alarm = await _dbContext.Alarms.Include(a => a.Task).FirstOrDefaultAsync(a => a.Id == id);
            if (alarm != null)
            {
                if (alarm.Task.User.UserName == username)
                {
                    _dbContext.Alarms.Remove(alarm);
                }
                else throw new ApplicationException("You cant edit this alarm");
            }
            else throw new ApplicationException("Task not found");

            await _dbContext.SaveChangesAsync();
        }
        public async Task Update(int id,string username,AlarmEditDto dto)
        {
            var alarm =await _dbContext.Alarms.Where(a => a.Task.User.UserName == username).FirstOrDefaultAsync(a => a.Id == id);
            if (alarm == null) throw new ApplicationException("the id is wrong or you dont have access to this alarm");
            alarm.DaysInWeek = dto.DaysInWeek;
            alarm.Date = dto.Date;
            alarm.Description = dto.Description;
            alarm.Time = dto.Description;
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetAlarm(Alarm alarm)
        {
            await _dbContext.Alarms.AddAsync(alarm);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Alarm>> GetAll(string username)
        {
            return await _dbContext.Alarms.Where(t => t.Task.User.UserName == username).Include(t => t.Task).ToListAsync();
        }
    }
}
