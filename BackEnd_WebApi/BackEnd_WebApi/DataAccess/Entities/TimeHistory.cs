namespace BackEnd_WebApi.DataAccess.Entities
{
    public class TimeHistory
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; } = DateTime.Now;
        public DateTime? ToDate { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public int ApplicationTaskId { get; set; } 
        public ApplicationTask? ApplicationTask { get; set; }
    } 
}
