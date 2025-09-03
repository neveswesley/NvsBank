using Microsoft.EntityFrameworkCore;
using NvsBank.Domain.Entities;

namespace NvsBank.Infrastructure.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Customer?> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Address)
            .WithOne(a => a.Customer)
            .HasForeignKey<Address>(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}