using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Database;

public class UnitOfWork : IUnitOfWork
{
    public readonly AppDbContext _context;

    public ICustomerRepository Customers { get; }
    public IAddressRepository Addresses { get; }
    public IAccountRepository Accounts { get; }

    public UnitOfWork(AppDbContext context, ICustomerRepository customers, IAddressRepository addresses, IAccountRepository accounts)
    {
        _context = context;
        Customers = customers;
        Addresses = addresses;
        Accounts = accounts;
    }

    public async Task Commit(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}