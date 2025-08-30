using NvsBank.Domain.Entities;

namespace NvsBank.Application.Interfaces;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer> GetByDocument(string document);
}