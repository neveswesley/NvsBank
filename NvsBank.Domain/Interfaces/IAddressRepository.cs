using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.Interfaces;

public interface IAddressRepository : IBaseRepository<Address>
{
    Task<PagedResult<Address>> GetAllWithCustomerAsync(int page, int pageSize);
    Task<Address> GetByIdWithCustomerAsync(Guid id);
}