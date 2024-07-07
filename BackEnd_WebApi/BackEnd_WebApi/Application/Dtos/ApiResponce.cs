using BackEnd_WebApi.Application.Interfaces;
using BackEnd_WebApi.Application.Services;

namespace BackEnd_WebApi.Application.Dtos
{
    public class ApiResponce<T> : ApiResponce
    {
        public ApiResponce(string username) : base(username)
        {
            
        }
        public T? Content { get; set; } 
    } 
    public class ApiResponce : BaseApiResponce
    {
        public ApiResponce(string username)
        {
            var AS = new AlarmService();
            Notify = AS.HaveNotify(username);
        }
        public int Notify { get; private set; } =0;
    }
    public class BaseApiResponce
    {
        public string Message { get; set; } = string.Empty;
        public bool Succeeded { get; set; } = false;
    } 
    public class BaseApiResponce<T> : BaseApiResponce
    {
        public T? Content { get; set; }
    }
}
