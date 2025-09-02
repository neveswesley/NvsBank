using System.Threading;
using System.Threading.Tasks;
using NvsBank.Application.Interfaces;

public interface IUnitOfWork
{
    ICustomerRepository Customers { get; }
    IAddressRepository Addresses { get; }
    Task Commit(CancellationToken cancellationToken);
}