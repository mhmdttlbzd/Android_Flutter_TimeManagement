using BackEnd_WebApi.Application.Dtos;

namespace BackEnd_WebApi.Application.Interfaces
{
    public interface IHistoryService
    {
        Task<bool> DeleteTimeHistory(int id,string username);
        Task<List<TimeHistoryResponceDto>> GetTaskHistory(string userId);
        Task<bool> AddTag(string name, int historyId);
        Task<List<TimeHistoryResponceDto>> GetTaskHistory(string userName, int categoryId, int tagId, string fromDate, string toDate);
        Task<List<GeneralResponceDto>> GetAllTags();
        Task<ApiResponce> Clear(string userName);
        Task CreateFakeHistory(string date, string fromTime, string toTime, int taskId);
    }
}
