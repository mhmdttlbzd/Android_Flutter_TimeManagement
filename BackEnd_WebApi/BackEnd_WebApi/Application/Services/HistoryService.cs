using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Interfaces;
using BackEnd_WebApi.DataAccess.Entities;
using BackEnd_WebApi.DataAccess.Interfaces;
using BackEnd_WebApi.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace BackEnd_WebApi.Application.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskHistoryRepository _taskHistoryRepository;
        private readonly ITagRepository _tagRepository;
        public HistoryService(UserManager<ApplicationUser> userManager, ITaskHistoryRepository taskHistoryRepository, ITagRepository tagRepository)
        {
            _userManager = userManager;
            _taskHistoryRepository = taskHistoryRepository;
            _tagRepository = tagRepository;
        }
        public async Task CreateFakeHistory(string date,string fromTime,string toTime,int taskId)
        {
            var dateT = GetDate(date,'/');
            var ft = GetTime(fromTime);
            var tt = GetTime(toTime);
            if (dateT == null) throw new ApplicationException("Enter Date Correctly");
            var res = new TimeHistory
            {
                ApplicationTaskId = taskId,
                FromDate = dateT + ft ?? DateTime.Now,
                ToDate = dateT + tt
            };
            await _taskHistoryRepository.Create(res);
        }

        private TimeSpan GetTime(string time)
        {
            try
            {
                var res = new TimeSpan();
                var split = time.Split(':');
                if (split.Length == 2) res = new TimeSpan(int.Parse(split[0]), int.Parse(split[1]),0);
                else if (split.Length == 1) res = new TimeSpan(int.Parse(split[0]), 0, 0);
                else throw new Exception();
                return res;
            }
            catch
            {
                throw new ApplicationException("enter time in 00:00 format");
            }
        }
        public async Task<bool> DeleteTimeHistory(int id, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return await _taskHistoryRepository.DeletTimeHistory(id, user?.Id);
        }


        public async Task<List<TimeHistoryResponceDto>> GetTaskHistory(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var tasks = await _taskHistoryRepository.GetTaskHistory(user?.Id ?? string.Empty);
            return MapTaskToHistoryDto(tasks);
        }
        public async Task<List<TimeHistoryResponceDto>> GetTaskHistory(string userName, int categoryId, int tagId, string fromDate, string toDate)
        {
            var from = GetDate(fromDate, '/');
            var to = GetDate(toDate, '/');
            var user = await _userManager.FindByNameAsync(userName);

            var tasks = await _taskHistoryRepository.GetTaskHistory(user?.Id ?? string.Empty);
            if (categoryId != -1) tasks = tasks.Where(t => t.CategoryId == categoryId).ToList();
            foreach (var task in tasks)
            {
                if (from != null) task.timeHistories = task.timeHistories.Where(h => h.FromDate >= from).ToList();
                if (to != null) task.timeHistories = task.timeHistories.Where(h => h.ToDate <= to).ToList();
            }

            var histories = MapTaskToHistoryDto(tasks, tagId);
            return histories;
        }

        private DateTime? GetDate(string date, char split)
        {

            if (string.IsNullOrEmpty(date)) return null;
            if (!Regex.IsMatch(date, @"^\d{4}" + split + @"\d{2}" + split + @"\d{2}")) throw new ApplicationException($"date must be in yyyy{split}mm{split}dd format");
            var splitDate = date.Split(split);
            var cal = new PersianCalendar();
            var res = cal.ToDateTime(int.Parse(splitDate[0]), int.Parse(splitDate[1]), int.Parse(splitDate[2]), 0, 0, 0, 0);
            if (res > DateTime.Now) throw new ApplicationException("date must be in past");
            return res;
        }

        private List<TimeHistoryResponceDto> MapTaskToHistoryDto(List<ApplicationTask> tasks, int tagId = -1)
        {
            var res = new List<TimeHistoryResponceDto>();
            foreach (var task in tasks)
            {
                var histories = MapTaskToHistoryDto(task, tagId);
                res.AddRange(histories);
            }
            return res;
        }
        private List<TimeHistoryResponceDto> MapTaskToHistoryDto(ApplicationTask task, int tagId)
        {
            var cal = new PersianCalendar();
            var res = new List<TimeHistoryResponceDto>();

            foreach (var h in task.timeHistories)
            {
                var t = h.ToDate ?? DateTime.Now;
                var time = t - h.FromDate;
                if (tagId != -1 && !h.Tags.Any(t => t.Id == tagId)) continue;
                var mo = cal.GetMonth(h.FromDate)<10 ? "0"+cal.GetMonth(h.FromDate) : ""+cal.GetMonth(h.FromDate);
                var day = cal.GetDayOfMonth(h.FromDate)<10 ? "0"+cal.GetDayOfMonth(h.FromDate) : ""+cal.GetDayOfMonth(h.FromDate);
                var r = new TimeHistoryResponceDto
                {
                    Date = cal.GetYear(h.FromDate) + "/" + mo  + "/" + day + " --- " + cal.GetDayOfWeek(h.FromDate),
                    FromTime = cal.GetHour(h.FromDate) + ":" + cal.GetMinute(h.FromDate),
                    ToTime = cal.GetHour(h.ToDate ?? DateTime.Now) + ":" + cal.GetMinute(h.ToDate ?? DateTime.Now),
                    Time = time.Hours + ":" + time.Minutes + ':' + time.Seconds,
                    TaskName = task.Name,
                    Id = h.Id
                };
                res.Add(r);
            }
            return res;
        }
        public async Task<bool> AddTag(string name, int historyId)
        {
            int? tagId = await _tagRepository.GetTagIdByName(name);
            if (tagId == null && !string.IsNullOrEmpty(name)) { tagId = await _tagRepository.CraeteTag(name); }
            if (tagId != null) { return await _tagRepository.AddToHistory(tagId ?? 0, historyId); }
            return false;
        }

        public async Task<List<GeneralResponceDto>> GetAllTags()
        {
            var tags = await _tagRepository.GetAll();
            var res = new List<GeneralResponceDto>();
            foreach (var tag in tags)
            {
                res.Add(new GeneralResponceDto
                {
                    Name = tag.Name,
                    Id = tag.Id
                });
            }
            return res;
        }
        public async Task<ApiResponce> Clear(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            await _taskHistoryRepository.Clear(user?.Id.ToString() ?? string.Empty);
            return new ApiResponce { Succeeded = true, Message = "" };
        }

    }
}
