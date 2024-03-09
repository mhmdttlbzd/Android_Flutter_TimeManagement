using Microsoft.AspNetCore.Identity;

namespace BackEnd_WebApi.DataAccess.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = "name";
        public string? Family { get; set;}
        public string ForgotPassword { get; set;} = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public List<ApplicationTask> Tasks { get; set; } = new List<ApplicationTask>();
    }
}
