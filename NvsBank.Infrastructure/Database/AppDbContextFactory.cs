using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NvsBank.Infrastructure.Database;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=WESLEY\\SQLEXPRESS;Database=NvsBank;User ID=sa;Password=1q2w3e4r5t@#;TrustServerCertificate=True;");
        return new AppDbContext(optionsBuilder.Options);
    }
}