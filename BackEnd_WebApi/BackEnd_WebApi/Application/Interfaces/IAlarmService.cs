using BackEnd_WebApi.Application.Dtos;
using System.Threading.Tasks;

namespace BackEnd_WebApi.Application.Interfaces
{
    public interface IAlarmService
    {
      
        Task SetAlarm(string date, string time, int taskId, int[] daysInWeek, string description);
        List<NotifyDto> GetNotifies(string username);
        Task<ApiResponce<List<AlarmDto> >> GetAlarms(string username);
        Task DeleteAlarm(int id, string username);
    }
}
