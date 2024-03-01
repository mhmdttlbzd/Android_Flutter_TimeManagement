namespace BackEnd_WebApi.Application.Dtos
{
    public class ApiResponce
    {
        public string Content { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool Succeeded { get; set; } = false;
    }
}
