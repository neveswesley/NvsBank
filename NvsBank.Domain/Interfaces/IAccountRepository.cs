using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.Interfaces;

public interface IAccountRepository
{
    Task<Account> CreateAsync(Account account);
    void UpdateAsync(Account account);
    Task<Account> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<Account>> GetPagedAsync(int page, int pageSize);
    Task<PagedResult<Account>> GetActiveAsync(int page, int pageSize);
    void InactiveAsync(Account account);
    Task<IEnumerable<Account>> GetAllAccountWithCustomer();
    
    void Deposit(Guid id, decimal amount);
    void Withdraw(Guid id, decimal amount);
    decimal GetBalance(Guid id);
}