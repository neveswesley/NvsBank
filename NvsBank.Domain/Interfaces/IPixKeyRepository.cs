using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Interfaces;

public interface IPixKeyRepository : IBaseRepository<PixArea>
{
    Task<string> GenerateUniqueEvPAsync(int maxAttempts = 5, CancellationToken cancellationToken = default);
    Task<PixArea> CreateEvPForAccountAsync(Guid accountId, int maxAttemps = 5);
    Task<bool> ExistsAsync(Guid accountId, PixKeyType type, CancellationToken cancellationToken);
    Task<PagedResult<PixArea>> GetPixKeysByIdAsync(Guid accountId, int page, int pageSize);
    void RemoveKeyValue(PixKeyType pixKeyType);
    Task<PixArea> GetPixKeyByIdAsync(string pixKeyId);
    Task<List<PixArea>> GetAllAsync();
}