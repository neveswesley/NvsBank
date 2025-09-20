using Microsoft.EntityFrameworkCore;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class PixKeyRepository : BaseRepository<PixArea>, IPixKeyRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<PixArea> _dbSet;

    public PixKeyRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<PixArea>();
    }

    public async Task<string> GenerateUniqueEvPAsync(int maxAttempts = 5, CancellationToken cancellationToken = default)
    {
        if (maxAttempts <= 0) throw new ArgumentOutOfRangeException(nameof(maxAttempts));

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var candidate = Guid.NewGuid().ToString("N");

            var exists = await Context.PixAreas
                .AsNoTracking()
                .AnyAsync(p => p.KeyValue == candidate, cancellationToken);

            if (!exists)
                return candidate;
        }

        throw new InvalidOperationException(
            $"Não foi possível gerar uma chave EVP única após {maxAttempts} tentativas.");
    }

    public async Task<PixArea> CreateEvPForAccountAsync(Guid accountId, int maxAttemps = 5)
    {
        var evp = await GenerateUniqueEvPAsync(maxAttemps);

        var pixKey = new PixArea
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            KeyType = PixKeyType.EVP,
            KeyValue = evp,
            Status = PixKeyStatus.Pending
        };

        Context.PixAreas.Add(pixKey);
        await Context.SaveChangesAsync();

        return pixKey;
    }

    public async Task<bool> ExistsAsync(Guid accountId, PixKeyType type, CancellationToken cancellationToken)
    {
        return await Context.PixAreas.AnyAsync(x => x.AccountId == accountId && x.KeyType == type && x.Status == PixKeyStatus.Active, cancellationToken);
    }

    public async Task<PagedResult<PixArea>> GetPixKeysByIdAsync(Guid accountId, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        
        var totalCount = await _dbSet.Where(x=>x.AccountId == accountId).CountAsync();

        var items = await _dbSet.Where(x => x.AccountId == accountId).Where(x=>x.Status == PixKeyStatus.Active).OrderBy(x=>x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<PixArea>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public void RemoveKeyValue(PixKeyType pixKeyType)
    {
        var key = _context.PixAreas.FirstOrDefault(x => x.KeyType == pixKeyType);
        if (key != null) key.Status = PixKeyStatus.Deleted;
    }

    public async Task<PixArea> GetPixKeyByIdAsync(string pixKeyId)
    {
        var pixKey = await _context.PixAreas.Include(x=>x.Account).FirstOrDefaultAsync(x => x.KeyValue == pixKeyId);
        if (pixKey == null)
            throw new ApplicationException("Pix key not found.");
        
        return pixKey;
    }

    public async Task<List<PixArea>> GetAllAsync()
    {
        var pixKeys = await _context.PixAreas.ToListAsync();
        return pixKeys;
    }
}