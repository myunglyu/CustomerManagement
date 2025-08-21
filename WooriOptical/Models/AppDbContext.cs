using Microsoft.EntityFrameworkCore;

namespace WooriOptical.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        // Add DbSet<T> properties here, e.g.:
        // public DbSet<Customer> Customers { get; set; }
    }
}
