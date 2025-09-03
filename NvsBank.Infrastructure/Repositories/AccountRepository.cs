using Microsoft.EntityFrameworkCore;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    public AccountRepository(AppDbContext context) : base(context)
    {
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