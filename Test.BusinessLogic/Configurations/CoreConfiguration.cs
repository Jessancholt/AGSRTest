using Microsoft.Extensions.DependencyInjection;
using Test.Core.Models;
using Test.Core.Services;
using Test.Core.Services.Interfaces;
using Test.Core.Settings;

namespace Test.Core.Configurations
{
    public static class CoreConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services, Action<CacheSettingsOptions> cacheSetup)
        {
            // services
            services.AddScoped<IPatientsService, PatientsService>();
            services.AddScoped<ICacheService<string, PatientContext>, CacheService<string, PatientContext>>();

            services.Configure(cacheSetup);

            return services;
        }
    }
}
