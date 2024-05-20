using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace Test.WebAPI.Infrastructure.Configurations
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
