using BackEnd_WebApi.Application.Interfaces;
using BackEnd_WebApi.Application.Services;

namespace BackEnd_WebApi.Application
{
    public  static class DependencyInjection
    {

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService,UserService>();

            return services;
        }
    }
}
