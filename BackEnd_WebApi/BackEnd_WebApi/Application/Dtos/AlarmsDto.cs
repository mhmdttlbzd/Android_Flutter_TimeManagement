namespace BackEnd_WebApi.Application.Dtos
{
    public class AlarmDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string TaskName { get; set; }
        public string Time { get; set; }
        public List<int> DaysInWeek { get; set; }
    }
    public class AlarmEditDto
    {
        public string Description { get; set; } = string.Empty; 
        public string? DaysInWeek { get; set; }
        public string Time { get; set; }
        public DateTime? Date { get; set; }
    }
}
