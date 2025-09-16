using Microsoft.EntityFrameworkCore;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Transaction> _dbSet;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<Transaction>();
    }

    public async Task<PagedResult<Transaction>> GetByAccountIdAsync(Guid accountId, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var totalCount = await _dbSet.Where(x=>x.AccountId == accountId).CountAsync();

        var items = await _dbSet.Where(x=>x.AccountId == accountId).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Transaction>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(Transaction transaction)
    {
        transaction.Id = Guid.NewGuid();
        transaction.Timestamp = DateTime.Now;
        transaction.CreatedDate = DateTime.Now;

        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }
}