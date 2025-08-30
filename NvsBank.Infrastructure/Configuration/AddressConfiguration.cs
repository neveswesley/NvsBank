using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsBank.Domain.Entities;

namespace NvsBank.Infrastructure.Configuration;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().HasColumnType("varchar(100)");
        builder.Property(x => x.Street).IsRequired().HasColumnType("varchar(200)");
        builder.Property(x => x.City).IsRequired().HasColumnType("varchar(50)");
        builder.Property(x => x.State).IsRequired().HasColumnType("varchar(2)");
        builder.Property(x => x.ZipCode).IsRequired().HasColumnType("varchar(8)");

        builder.HasOne(a =>a.Customer).WithOne(a=>a.Address).HasForeignKey<Customer>(c=>c.AddressId);
    }
}