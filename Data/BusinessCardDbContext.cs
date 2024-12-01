using BusinessCardManagerAPI.Data.Configurations;
using BusinessCardManagerAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardManagerAPI.Data
{
    public class BusinessCardDbContext : DbContext
    {
        public DbSet<BusinessCard> BusinessCards { get; set; }

        public BusinessCardDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BusinessCardConfiguration).Assembly);
        }
    }
}
