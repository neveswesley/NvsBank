using Microsoft.EntityFrameworkCore;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Application.Shared.Extras;

using System;
using System.Threading;
using System.Threading.Tasks;

public class PixKeyService
{
    private readonly AppDbContext _db;

    public PixKeyService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<string> GenerateUniqueEvPAsync(int maxAttempts = 5, CancellationToken cancellationToken = default)
    {
        if (maxAttempts <= 0) throw new ArgumentOutOfRangeException(nameof(maxAttempts));

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var candidate = Guid.NewGuid().ToString("N");

            var exists = await _db.PixAreas
                .AsNoTracking()
                .AnyAsync(p => p.KeyValue == candidate, cancellationToken);

            if (!exists)
                return candidate;
        }

        throw new InvalidOperationException(
            $"Não foi possível gerar uma chave EVP única após {maxAttempts} tentativas.");
    }

    public async Task<PixArea> CreateEvPForAccountAsync(Guid accountId, int maxAttempts = 5,
        CancellationToken cancellationToken = default)
    {
        var evp = await GenerateUniqueEvPAsync(maxAttempts, cancellationToken);

        var pixKey = new PixArea
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            KeyType = PixKeyType.EVP,
            KeyValue = evp,
            Status = PixKeyStatus.Pending
        };

        _db.PixAreas.Add(pixKey);
        await _db.SaveChangesAsync(cancellationToken);

        return pixKey;
    }
}