using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NvsBank.Infrastructure.Configuration;

public class TransactionConfiguration : IEntityTypeConfiguration<Domain.Entities.Transaction>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Transaction> builder)
    {
        builder.ToTable("Transactions");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x=>x.AccountId).IsRequired();
        builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x=>x.NewBalance).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.OldBalance).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x=>x.TransactionType).IsRequired();
        builder.Property(x=>x.Description).IsRequired();
        builder.Property(x=>x.Timestamp).IsRequired();
    }
}