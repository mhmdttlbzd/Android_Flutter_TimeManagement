
using BackEnd_WebApi.Application.Dtos;
using System.Net;
using System.Text.Json;

namespace BackEnd_WebApi.Application.Middleware
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);

            }

            catch (ApplicationException e) 
            {
                var res = new ApiResponce()
                {
                    Message = e.Message
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(JsonSerializer.Serialize(res));
                
            }
            catch (Exception e)
            {
                var res = new ApiResponce()
                {
                    Message = "error"
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(JsonSerializer.Serialize(res));

            }

        }
    }
}
