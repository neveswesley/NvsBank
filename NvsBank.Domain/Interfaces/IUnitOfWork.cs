using System.Threading;
using System.Threading.Tasks;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Interfaces;

public interface IUnitOfWork
{
    ICustomerRepository Customers { get; }
    IAddressRepository Addresses { get; }
    IAccountRepository Accounts { get; }
    Task Commit(CancellationToken cancellationToken);
}