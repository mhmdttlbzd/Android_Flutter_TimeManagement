using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace BackEnd_WebApi.Application.Dtos
{
    public class LoginDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email cant be null")]
        public string UserName { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cant be null")]
        [MinLength(3,ErrorMessage = "Password must have at least 3 characters")]
        public string Password { get; set; }
    }
}
