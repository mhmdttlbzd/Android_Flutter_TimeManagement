namespace BackEnd_WebApi.DataAccess.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = "TagName";
        public List<TimeHistory> TimeHistories { get; set; } = new List<TimeHistory>(); 

    }
}
