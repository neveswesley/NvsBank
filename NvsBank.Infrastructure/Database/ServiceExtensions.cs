using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Repositories;

namespace NvsBank.Infrastructure.Database;

public static class ServiceExtensions
{
    public static void ConfigurePersistenceApp(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        
        services.AddScoped<IAddressRepository, AddressRepository>();
        
        services.AddScoped<IAccountRepository, AccountRepository>();
        
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        
        services.AddScoped<IBankSlipRepository, BankSlipRepository>();
        
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        
        services.AddScoped<IPixKeyRepository, PixKeyRepository>();
        
    }
}