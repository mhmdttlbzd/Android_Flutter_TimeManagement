namespace BackEnd_WebApi.DataAccess.Entities
{
    public class Alarm
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public ApplicationTask Task { get; set; }
        public int TaskId { get; set; }
        public string? DaysInWeek { get; set; }
        public string Time { get; set; }
        public DateTime? Date { get; set; }
    }
}
