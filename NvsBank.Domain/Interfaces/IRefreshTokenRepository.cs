using NvsBank.Domain.Entities;

namespace NvsBank.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken);
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
    Task UpdateAsync(RefreshToken token, CancellationToken cancellationToken);
}