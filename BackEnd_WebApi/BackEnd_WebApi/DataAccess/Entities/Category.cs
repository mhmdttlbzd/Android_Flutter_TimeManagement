namespace BackEnd_WebApi.DataAccess.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ApplicationTask> Tasks { get; set;} = new List<ApplicationTask>();
    } 
}
