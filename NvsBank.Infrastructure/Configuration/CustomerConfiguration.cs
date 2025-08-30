using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsBank.Domain.Entities;

namespace NvsBank.Infrastructure.Configuration;

public class CustomerConfiguration : IEntityTypeConfiguration<Domain.Entities.Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        builder.Property(c => c.FullName).IsRequired().HasColumnType("varchar(100)");
        builder.Property(c => c.Type).IsRequired().HasConversion<string>().HasColumnType("varchar(30)");
        builder.Property(c => c.DocumentNumber).IsRequired().HasColumnType("varchar(50)");
        builder.Property(c => c.BirthDate).HasColumnType("datetime");
        builder.Property(c => c.FoundationDate).HasColumnType("datetime");

        builder.HasOne(c => c.Address).WithOne(c => c.Customer).HasForeignKey<Address>(a => a.CustomerId);

        builder.Property(c => c.PhoneNumber).IsRequired().HasColumnType("varchar(20)");
        builder.Property(c => c.Email).IsRequired().HasColumnType("varchar(50)");
        builder.Property(c => c.CustomerStatus).IsRequired().HasConversion<string>().HasColumnType("varchar(50)");
    }
}