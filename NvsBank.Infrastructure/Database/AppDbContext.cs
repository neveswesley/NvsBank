using Microsoft.EntityFrameworkCore;
using NvsBank.Domain.Entities;

namespace NvsBank.Infrastructure.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Customer> Customers { get; set; }
}