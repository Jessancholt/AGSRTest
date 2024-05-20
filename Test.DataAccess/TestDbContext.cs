using Microsoft.EntityFrameworkCore;
using Test.DataAccess.Entities;

namespace Test.DataAccess
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Given> GivenNames { get; set; }
    }
}
