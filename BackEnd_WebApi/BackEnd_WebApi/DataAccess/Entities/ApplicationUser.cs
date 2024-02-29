using Microsoft.AspNetCore.Identity;

namespace BackEnd_WebApi.DataAccess.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<ApplicationTask> Tasks { get; set; } = new List<ApplicationTask>();
    }
}
