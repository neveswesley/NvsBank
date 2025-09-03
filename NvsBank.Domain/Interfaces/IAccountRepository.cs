using NvsBank.Domain.Entities;

namespace NvsBank.Application.Interfaces;

public interface IAccountRepository : IBaseRepository<Account>
{
    Task<IEnumerable<Account>> GetAllAccountWithCustomer();
    
    void AddBalance(Guid id, decimal amount);
    void Withdraw(Guid id, decimal amount);
    decimal GetBalance(Guid id);
}