using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WooriOptical.Models;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Order> Orders { get; set; }
}

