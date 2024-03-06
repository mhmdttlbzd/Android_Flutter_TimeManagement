using System.ComponentModel.DataAnnotations;

namespace BackEnd_WebApi.Application.Dtos
{
    public class ResetPasswordDto
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cant be null")]
        [MinLength(3, ErrorMessage = " correntPassword must have at least 3 characters")]
        public string correntPassword { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cant be null")]
        [MinLength(3, ErrorMessage = " newPassword must have at least 3 characters")]
        public string newPassword { get; set; } 
    }
}
