using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RefreshToken = NvsBank.Domain.Entities.RefreshToken;


namespace NvsBank.Infrastructure.Database;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<BankSlip> BankSlips { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PixKey> PixKeys { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // raiz
        modelBuilder.Entity<Person>(b =>
        {
            b.HasKey(p => p.Id);
            b.HasOne(p => p.User)
                .WithOne(u => u.Person)
                .HasForeignKey<Person>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // TPT
        modelBuilder.Entity<Customer>().ToTable("Customers");
        modelBuilder.Entity<Employee>().ToTable("Employees");

        // Customer ↔ Address
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Address)
            .WithOne(a => a.Customer)
            .HasForeignKey<Address>(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Owned Types
        modelBuilder.Entity<Customer>().OwnsOne(c => c.Limits);
        modelBuilder.Entity<Employee>().OwnsOne(e => e.Limits);

        // User
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
        modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
    }
}
