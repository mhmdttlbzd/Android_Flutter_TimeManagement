namespace BackEnd_WebApi.Application.Dtos
{
    public class NotifyDto
    {
        public string Description { get; set; }
        public string TaskName { get; set; }
        public int TaskId { get; set; }
        public int MinuteLeft { get; set; }

    }
}
