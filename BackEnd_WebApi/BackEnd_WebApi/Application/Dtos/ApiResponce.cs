namespace BackEnd_WebApi.Application.Dtos
{
    public class ApiResponce<T>
    {
        public T? Content { get; set; } 
        public string Message { get; set; } = string.Empty;
        public bool Succeeded { get; set; } = false;
    } 
    public class ApiResponce
    {
        public string Message { get; set; } = string.Empty;
        public bool Succeeded { get; set; } = false;
    }
}
