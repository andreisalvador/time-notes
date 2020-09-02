using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
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
                options.UseNpgsql(@"User ID = avycvcypeqolyt;Password=8a3055e9720308926268373a5cd19d710b57ce7c5bbf87a480624d0453b54665;Server=ec2-3-91-139-25.compute-1.amazonaws.com;Port=5432;Database=d1lmn1982hg9ok;"));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        private static void AddContexts(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TimeNotesContext>(options =>
            options.UseNpgsql(@"User ID = avycvcypeqolyt;Password=8a3055e9720308926268373a5cd19d710b57ce7c5bbf87a480624d0453b54665;Server=ec2-3-91-139-25.compute-1.amazonaws.com;Port=5432;Database=d1lmn1982hg9ok;"));

            services.AddScoped<TimeNotesContext>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IHourPointsRepository, HourPointsRepository>();
            services.AddScoped<IHourPointConfigurationsRepository, HourPointConfigurationsRepository>();
        }
    }
}
