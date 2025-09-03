using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;
using NvsBank.Domain.Entities;

namespace NvsBank.Infrastructure.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Account");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.Property(x=>x.AccountNumber).IsRequired().HasColumnType("varchar(100)");
        builder.Property(x=>x.Branch).IsRequired().HasColumnType("varchar(100)");
        builder.Property(x => x.AccountType).IsRequired().HasConversion<string>().HasColumnType("varchar(30)");
        builder.Property(x => x.Balance).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.OverdraftLimit).HasColumnType("decimal(18,2)");
        builder.Property(x=>x.OpeningDate).IsRequired();
        builder.Property(x => x.ClosingDate);
        builder.Property(x => x.Status).IsRequired().HasConversion<string>().HasColumnType("varchar(30)");
        builder.Property(x=>x.CustomerId).IsRequired();
        
        builder.HasOne<Customer>(x => x.Customer).WithMany(c => c.Account).HasForeignKey(a => a.CustomerId);
    }
}