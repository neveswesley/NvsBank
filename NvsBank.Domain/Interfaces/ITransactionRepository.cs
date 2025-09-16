using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Infrastructure.Repositories;

public interface ITransactionRepository
{
    Task<PagedResult<Transaction>> GetByAccountIdAsync(Guid accountId, int page, int pageSize);
    Task AddAsync(Transaction transaction);
}