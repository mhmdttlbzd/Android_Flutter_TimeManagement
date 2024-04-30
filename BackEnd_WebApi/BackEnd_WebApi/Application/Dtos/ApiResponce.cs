namespace BackEnd_WebApi.Application.Dtos
{
    public class ApiResponce<T> : ApiResponce
    {
        public T? Content { get; set; } 
    } 
    public class ApiResponce
    {
        public string Message { get; set; } = string.Empty;
        public bool Succeeded { get; set; } = false;
        public bool Notify { get; set; } = false;
    }
}
