using BackEnd_WebApi.Application.Interfaces;
using BackEnd_WebApi.Application.Services;
using BackEnd_WebApi.DataAccess.Interfaces;
using BackEnd_WebApi.DataAccess.Repositories;

namespace BackEnd_WebApi.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddDbContext<ApplicationDbContext>();

            return services;
        }
    }
}
