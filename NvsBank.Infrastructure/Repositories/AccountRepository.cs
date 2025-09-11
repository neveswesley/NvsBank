using Microsoft.EntityFrameworkCore;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Account> _dbSet;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<Account>();
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

    
    public async Task<PagedResult<Account>> GetPagedAsync(int page, int pageSize)
    {
        
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        
        var totalCount = await _dbSet.CountAsync();

        var items = await _dbSet.OrderBy(x=>x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Account>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<IEnumerable<Account>> GetActiveAsync(CancellationToken cancellationToken)
    {
        var accounts = await _context.Accounts.Where(x => x.AccountStatus == AccountStatus.Active)
            .ToListAsync(cancellationToken);
        return accounts;
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

    public void Deposit(Guid id, decimal amount)
    {
        var result = _context.Accounts.FirstOrDefaultAsync(x => x.Id == id).Result;
        if (result != null)
            result.Deposit(amount);
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