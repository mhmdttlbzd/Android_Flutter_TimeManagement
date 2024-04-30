using System.ComponentModel.DataAnnotations;

namespace BackEnd_WebApi.Application.Dtos
{
    public class RegisterInputDto
    {
        [Required(AllowEmptyStrings = false,ErrorMessage ="Name cant be null")]
        public string UserName { get; set; }
        public int PhoneNumber { get; set; } 

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email cant be null")]
        [EmailAddress(ErrorMessage = "Email is incorrect")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cant be null")]
        [MinLength(3, ErrorMessage = "Password must have at least 3 characters")]
        public string Password { get; set; }

    }    
}
