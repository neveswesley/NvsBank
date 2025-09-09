using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsBank.Domain.Entities;

namespace NvsBank.Infrastructure.Configuration;

public class BankSlipConfiguration : IEntityTypeConfiguration<BankSlip>
{
    public void Configure(EntityTypeBuilder<BankSlip> builder)
    {
        builder.ToTable("BankSlips");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x=>x.DigitableLine).IsRequired();
        builder.Property(x=>x.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x=>x.PayeeId).IsRequired().HasColumnType("varchar(50)");
        builder.Property(x=>x.PayerId).IsRequired().HasColumnType("varchar(50)");
        builder.Property(x => x.IsPaid).IsRequired().HasColumnType("bit");
    }
}