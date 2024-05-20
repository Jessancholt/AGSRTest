using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Test.Core.Configurations;
using Test.Core.Models;
using Test.Core.Services.Interfaces;
using Test.DataAccess;
using Test.DataAccess.Configurations;
using Test.Demo.Models;
using Test.Demo.Models.Predefined;
using Test.WebAPI.Infrastructure.Configurations;

var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory + "./../../..")
                .AddJsonFile(DemoConfig.CONFIG_FILE, optional: false);

IConfiguration config = builder.Build();

var serviceProvider = new ServiceCollection()
            .ConfigureDataAccess(option =>
            {
                option.UseLazyLoadingProxies();
                option.UseSqlite(config.GetConnectionString(Config.DB_SQLITE) ?? string.Empty);
            })
            .AddServices(option =>
            {
                config.GetSection(Config.CACHE_SETTINGS).Bind(option);
            })
            .Configure<DemoCreateModel>(options =>
            {
                config.GetSection(DemoConfig.DEMO).Bind(options);
            })
            .AddMemoryCache()
            .AddLogging()
            .BuildServiceProvider();

var options = serviceProvider.GetService<IOptions<DemoCreateModel>>().Value;
await using var dbContext = serviceProvider.GetRequiredService<TestDbContext>();
await dbContext.Database.MigrateAsync();

var service = serviceProvider.GetService<IPatientsService>();

var patients = new List<PatientCreateModel>();
var rnd = new Random();
for (int i = 0; i < options.Count; i++)
{
    string birthDate = null;
    if (DateTime.TryParse(options.BirthDate, out var start))
    {
        int range = (DateTime.Today - start).Days;
        birthDate = start
            .AddDays(rnd.Next(range))
            .AddHours(rnd.Next(0, 24))
            .AddMinutes(rnd.Next(0, 60))
            .AddSeconds(rnd.Next(0, 60))
            .ToString();
    }

    var model = new PatientCreateModel()
    {
        Use = options.Use[rnd.Next(options.Use.Count)],
        Family = options.Family[rnd.Next(options.Family.Count)],
        Given = new List<string>
        {
            options.Given[rnd.Next(options.Given.Count)],
            options.Given[rnd.Next(options.Given.Count)],
        },
        Gender = options.Gender[rnd.Next(options.Gender.Count)],
        BirthDate = birthDate,
    };

    patients.Add(model);
}

await service.CreateAsync(patients, default);