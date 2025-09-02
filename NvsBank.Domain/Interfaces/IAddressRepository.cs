using NvsBank.Domain.Entities;

namespace NvsBank.Application.Interfaces;

public interface IAddressRepository : IBaseRepository<Address>
{
    Task<List<Address>> GetAllWithCustomerAsync();
    Task<Address> GetByIdWithCustomerAsync(Guid id);
}