namespace BackEnd_WebApi.Application.Dtos
{
    public class TimeHistoryResponceDto
    {
        public string Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Time { get; set; } 
        public string TaskName {  get; set; }
        public int Id {  get; set; }
    }
}
