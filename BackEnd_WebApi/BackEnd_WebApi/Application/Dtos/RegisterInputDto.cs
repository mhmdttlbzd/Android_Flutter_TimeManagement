using System.ComponentModel.DataAnnotations;

namespace BackEnd_WebApi.Application.Dtos
{
    public class RegisterInputDto
    {
        [Required]
        public string Name { get; set; }
        public string Family { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }    
}
