using Microsoft.EntityFrameworkCore;

namespace WooriOptical.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Order> Orders { get; set; }
}

