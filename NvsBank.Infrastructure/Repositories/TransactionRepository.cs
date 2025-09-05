using Microsoft.EntityFrameworkCore;
using NvsBank.Domain.Entities;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(AppDbContext context) : base(context)
    {
        
    }

    public async Task<List<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        return await _context.Transactions.Where(t => t.AccountId == accountId).OrderBy(t => t.Timestamp).ToListAsync();
    }

    public async Task AddAsync(Transaction transaction)
    {
        transaction.Id = Guid.NewGuid();
        transaction.Timestamp = DateTime.Now;
        
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }
}