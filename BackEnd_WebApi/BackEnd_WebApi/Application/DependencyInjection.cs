﻿using BackEnd_WebApi.Application.Interfaces;
using BackEnd_WebApi.Application.Services;

namespace BackEnd_WebApi.Application
{
    public  static class DependencyInjection
    {

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<ITaskService,TaskService>();
            services.AddScoped<ICategoryService,CategoryService>();
            services.AddScoped<IHistoryService,HistoryService>();
            services.AddScoped<IAlarmService, AlarmService>();
            return services;
        }
    }
}
