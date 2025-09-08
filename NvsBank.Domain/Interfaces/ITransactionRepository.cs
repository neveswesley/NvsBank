using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;

namespace NvsBank.Infrastructure.Repositories;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetByAccountIdAsync(Guid accountId);
    Task AddAsync(Transaction transaction);
}