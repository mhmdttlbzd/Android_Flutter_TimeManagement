namespace BackEnd_WebApi.Application.Dtos
{
    public class RegisterResponce
    {
        public string Token { get; set; }
        public string RefreshToken {  get; set; }   
        public string  ForgotPassword { get; set; }
    }
    public class LoginResponce
    {
        public string Token { get; set; }
        public string RefreshToken {  get; set; }   
    }
}
