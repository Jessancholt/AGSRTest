using Microsoft.OpenApi.Models;

namespace Test.WebAPI.Infrastructure.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Test.WebAPI - API",
                    Version = "v1",
                    Description = "Documentation of API"
                });
                setup.EnableAnnotations();
            });

            return services;
        }

        public static WebApplication SwaggerConfig(this WebApplication app)
        {
            var isSwaggerEnabledString = app.Configuration["IsSwaggerEnabled"];

            if (bool.TryParse(isSwaggerEnabledString, out var isSwaggerEnabled))
            {
                if (isSwaggerEnabled)
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
            }

            return app;
        }
    }
}
