using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using TimeNotas.App.ProfileMaps;
using TimeNotas.Infrastructure.Data.Identity;
using TimeNotes.Data.Repository;
using TimeNotes.Domain;
using TimeNotes.Domain.Data.Interfaces;
using TimeNotes.Domain.Services;
using TimeNotes.Infrastructure.Cache;
using TimeNotes.Infrastructure.Components.Exports.Excel;
using TimeNotes.Infrastructure.Data;

namespace TimeNotas.App.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            AddContexts(services, configuration, webHostEnvironment);
            AddIdentity(services, configuration, webHostEnvironment);
            AddRepositories(services);
            AddMappings(services);
            AddDomainServices(services);
            AddInfrastrucutreComponents(services);
            AddRedisCache(services, webHostEnvironment);
        }

        private static void AddRedisCache(IServiceCollection services, IWebHostEnvironment webHostEnvironment)
        {
            string redisCloudUrl = webHostEnvironment.IsDevelopment() ? "localhost:6379" : Environment.GetEnvironmentVariable("REDISCLOUD_URL");

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = redisCloudUrl;
                options.InstanceName = "REDIS CACHE";
            });

            services.AddSingleton<RedisCache>();
        }

        private static void AddInfrastrucutreComponents(IServiceCollection services)
        {
            services.AddSingleton(typeof(ExcelExport<>));
        }

        private static void AddDomainServices(IServiceCollection services)
        {
            services.AddScoped<HourPointsServices>();
        }

        private static void AddMappings(IServiceCollection services)
        {
            services.AddAutoMapper(new Assembly[] { typeof(TimeEntryProfile).Assembly }, ServiceLifetime.Singleton);
        }

        private static void AddIdentity(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {   
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(webHostEnvironment.IsDevelopment() ? configuration.GetConnectionString("DefaultConnection") : Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")));
            services.AddDefaultIdentity<TimeNotesUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        private static void AddContexts(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            services.AddDbContext<TimeNotesContext>(options =>
            options.UseNpgsql(webHostEnvironment.IsDevelopment() ? configuration.GetConnectionString("DefaultConnection") : Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")));

            services.AddScoped<TimeNotesContext>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IHourPointsRepository, HourPointsRepository>();
            services.AddScoped<IHourPointConfigurationsRepository, HourPointConfigurationsRepository>();
        }
    }
}
