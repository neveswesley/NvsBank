using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsBank.Domain.Entities;

namespace NvsBank.Infrastructure.Configuration;

public class PixKeyConfiguration : IEntityTypeConfiguration<Domain.Entities.PixKey>
{
    public void Configure(EntityTypeBuilder<PixKey> builder)
    {
        builder.ToTable("PixKeys");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x=>x.KeyType).IsRequired();
        builder.Property(x=>x.KeyValue).IsRequired().HasColumnType("varchar(100)");
        builder.HasOne(x => x.Account).WithMany(x=>x.PixKey).HasForeignKey(x=>x.AccountId);
        
        builder.HasIndex(x=>x.KeyValue).IsUnique();
    }
}