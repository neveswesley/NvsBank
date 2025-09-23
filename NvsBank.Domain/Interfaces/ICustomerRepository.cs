using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Domain.Interfaces;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer> GetByDocument(string document);
    Task<bool> ExistsByEmailAsync(string email);
    Task<PagedResult<Customer>> GetAllWithAddressAsync(int page, int pageSize);
    Task<Customer> GetByIdWithAddressAsync(Guid id);
    Task<Customer> GetByIdWithAccountAsync(Guid id);
    Task<Customer> GetByDocumentWithAddressAsync(string document);
}