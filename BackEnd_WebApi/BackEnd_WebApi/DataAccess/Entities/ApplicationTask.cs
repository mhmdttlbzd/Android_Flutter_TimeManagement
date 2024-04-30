namespace BackEnd_WebApi.DataAccess.Entities
{
    public class ApplicationTask
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;    
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public ApplicationUser? User { get; set; } 
        public string UserId { get; set; } = string.Empty;
        public List<Alarm> Alarms { get; set; } = new List<Alarm>();
        public List<TimeHistory> timeHistories { get; set; } = new List<TimeHistory>(); 
    }
}
