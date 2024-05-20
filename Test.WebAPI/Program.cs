using Microsoft.EntityFrameworkCore;
using Test.Core.Configurations;
using Test.DataAccess.Configurations;
using Test.WebAPI.Infrastructure.Configurations;
using Test.WebAPI.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddFluentValidation();
builder.Services.ConfigureDataAccess(option =>
{
    option.UseLazyLoadingProxies();
    option.UseSqlite(builder.Configuration.GetConnectionString(Config.DB_SQLITE));
});
builder.Services.AddServices(option =>
{
    builder.Configuration.GetSection(Config.CACHE_SETTINGS).Bind(option);
});

var app = builder.Build();

app.SwaggerConfig();

app.UseHttpsRedirection();

app.MapControllers();

app.ConfigureExceptionHandler();

app.Run();
