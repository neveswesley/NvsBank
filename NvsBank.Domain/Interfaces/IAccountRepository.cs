using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.Interfaces;

public interface IAccountRepository
{
    Task<Account> CreateAsync(Account account);
    void UpdateAsync(Account account);
    Task<Account> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Account>> GetAllAsync(CancellationToken cancellationToken);
    void InactiveAsync(Account account);
    Task<IEnumerable<Account>> GetAllAccountWithCustomer();
    
    void AddBalance(Guid id, decimal amount);
    void Withdraw(Guid id, decimal amount);
    decimal GetBalance(Guid id);
}