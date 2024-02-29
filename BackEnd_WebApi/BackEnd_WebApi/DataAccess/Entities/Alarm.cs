namespace BackEnd_WebApi.DataAccess.Entities
{
    public class Alarm
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public ApplicationTask Task { get; set; } = new ApplicationTask();
        public int TaskId {  get; set; }
    }  
}
