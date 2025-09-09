using NvsBank.Domain.Entities;

namespace NvsBank.Application.Interfaces;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer> GetByDocument(string document);
    Task<bool> ExistsByEmailAsync(string email);
    Task<Customer> GetByIdAsync(Guid addressCustomerId);
    Task<List<Customer>> GetAllWithAddressAsync();
    Task<Customer> GetByIdWithAddressAsync(Guid id);
    Task<Customer> GetByDocumentWithAddressAsync(string document);
    Task<Customer> DeleteAddress(Guid id);
    void InactiveAsync(Customer customer);
    void ActiveAsync(Customer customer);
    void SuspendAsync(Customer customer);
    void BlockAsync(Customer customer);
    void CloseAsync(Customer customer);
    
}