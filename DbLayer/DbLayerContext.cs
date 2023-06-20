using Microsoft.EntityFrameworkCore;
using StarkovTestApp.Models;

namespace StarkovTestApp.DbLayer
{
    public sealed class DbLayerContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbLayerContext(IConfiguration configuration)
        {
            _configuration = configuration;
            Database.EnsureCreated();
        } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ConnectionString"));
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
    }
}
