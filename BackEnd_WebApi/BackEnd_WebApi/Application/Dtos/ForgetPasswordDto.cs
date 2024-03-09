using System.ComponentModel.DataAnnotations;

namespace BackEnd_WebApi.Application.Dtos
{
    public class ForgetPasswordDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email cant be null")]
        [EmailAddress(ErrorMessage = "Email is incorrect")]
        public string Email {  get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cant be null")]
        [MinLength(3, ErrorMessage = "Password must have at least 3 characters")]
        public string NewPassword { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "ForgotPassword cant be null")]
        [MinLength(5, ErrorMessage = "ForgotPassword must have at least 5 characters")]
        public string ForgotPassword { get; set; }
    }
}
