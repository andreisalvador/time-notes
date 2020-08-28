using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TimeNotas.App.Data;
using TimeNotas.App.ProfileMaps;
using TimeNotes.Data;
using TimeNotes.Data.Repository;
using TimeNotes.Domain.Data.Interfaces;
using TimeNotes.Domain.Services;

namespace TimeNotas.App.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddContexts(services, configuration);
            AddIdentity(services, configuration);
            AddRepositories(services);
            AddMappings(services);
            AddDomainServices(services);
        }

        private static void AddDomainServices(IServiceCollection services)
        {
            services.AddScoped<HourPointsServices>();
        }

        private static void AddMappings(IServiceCollection services)
        {
            services.AddAutoMapper(new Assembly[] { typeof(TimeEntryProfile).Assembly }, ServiceLifetime.Singleton);
        }

        private static void AddIdentity(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                         configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        private static void AddContexts(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TimeNotesContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<TimeNotesContext>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IHourPointsRepository, HourPointsRepository>();
            services.AddScoped<IHourPointConfigurationsRepository, HourPointConfigurationsRepository>();
        }
    }
}
