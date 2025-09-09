using Microsoft.EntityFrameworkCore;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Account> CreateAsync(Account account)
    {
        account.CreatedDate = DateTime.Now;
        await _context.Accounts.AddAsync(account);
        return account;
    }

    public void UpdateAsync(Account account)
    {
        account.ModifiedDate = DateTime.Now;
        _context.Accounts.Update(account);
    }

    public async Task<Account> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        return account;
    }

    public async Task<IEnumerable<Account>> GetAllAsync(CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.ToListAsync();
        return account;
    }
    
    public void InactiveAsync(Account account)
    {
        account.DeletedDate = DateTime.Now;
        account.AccountStatus = AccountStatus.Closed;
        _context.Accounts.Update(account);
    }

    public async Task<IEnumerable<Account>> GetAllAccountWithCustomer()
    {
        var account = await _context.Accounts.Include(x => x.Customer).ToListAsync();
        return account;
    }

    public void AddBalance(Guid id, decimal amount)
    {
        var result = _context.Accounts.FirstOrDefaultAsync(x => x.Id == id).Result;
        if (result != null)
            result.AddBalance(amount);
    }

    public void Withdraw(Guid id, decimal amount)
    {
        var result = _context.Accounts.FirstOrDefaultAsync(x => x.Id == id).Result;
        if (result != null)
            result.Withdraw(amount);
    }

    public decimal GetBalance(Guid id)
    {
        var account = _context.Accounts.FirstOrDefaultAsync(x => x.Id == id).Result;
        if (account != null)
            return account.Balance;

        return 0;
    }
}