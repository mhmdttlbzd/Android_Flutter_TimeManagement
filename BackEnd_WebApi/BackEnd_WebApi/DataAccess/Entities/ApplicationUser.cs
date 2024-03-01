using Microsoft.AspNetCore.Identity;

namespace BackEnd_WebApi.DataAccess.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = "name";
        public string? Family { get; set;}
        public List<ApplicationTask> Tasks { get; set; } = new List<ApplicationTask>();
    }
}
