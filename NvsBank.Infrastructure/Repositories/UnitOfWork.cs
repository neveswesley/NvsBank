using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using NvsBank.Application.Interfaces;
using NvsBank.Infrastructure.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public ICustomerRepository Customers { get; }
    public IAddressRepository Addresses { get; }
    public IAccountRepository Accounts { get; }

    public UnitOfWork(AppDbContext context,
        ICustomerRepository customerRepository,
        IAddressRepository addressRepository)
    {
        _context = context;
        Customers = customerRepository;
        Addresses = addressRepository;
    }

    public async Task Commit(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}