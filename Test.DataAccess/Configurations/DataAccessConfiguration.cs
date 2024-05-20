using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test.DataAccess.Entities;
using Test.DataAccess.Settings;
using Test.DataAccess.Storages;
using Test.DataAccess.Storages.Finders;
using Test.DataAccess.Storages.Finders.Interfaces;
using Test.DataAccess.Storages.Interfaces;

namespace Test.DataAccess.Configurations
{
    public static class DataAccessConfiguration
    {
        public static IServiceCollection ConfigureDataAccess(this IServiceCollection services, Action<DbContextOptionsBuilder> dbSetup)
        {
            // uow
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //finders
            services.AddScoped<IFinder<Patient>, Finder<Patient>>();
            services.AddScoped<IFinder<Given>, Finder<Given>>();

            // repositories
            services.AddScoped<IRepository<Patient>, Repository<Patient>>();
            services.AddScoped<IRepository<Given>, Repository<Given>>();

            services.AddDbContext<TestDbContext>(dbSetup);

            return services;
        }
    }
}
