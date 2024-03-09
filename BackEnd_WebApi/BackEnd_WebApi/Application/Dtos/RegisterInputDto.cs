using System.ComponentModel.DataAnnotations;

namespace BackEnd_WebApi.Application.Dtos
{
    public class RegisterInputDto
    {
        [Required(AllowEmptyStrings = false,ErrorMessage ="Name cant be null")]
        public string Name { get; set; }
        public string Family { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email cant be null")]
        [EmailAddress(ErrorMessage = "Email is incorrect")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cant be null")]
        [MinLength(3, ErrorMessage = "Password must have at least 3 characters")]
        public string Password { get; set; }

        [Compare("Password",ErrorMessage ="The passwords dont match")]
        public string ConfirmPassword { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "SecondPassword cant be null")]
        [MinLength(6, ErrorMessage = "Password must have at least 6 characters")]
        public string SecondPassword {  get; set; }
    }    
}
