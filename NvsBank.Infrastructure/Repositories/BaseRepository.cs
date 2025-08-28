using Microsoft.EntityFrameworkCore;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    
    private readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<T> CreateAsync(T entity)
    {
        entity.CreatedDate = DateTime.Now;
        await _context.AddAsync(entity);
        return entity;
    }

    public void UpdateAsync(T entity)
    {
        entity.ModifiedDate = DateTime.Now;
        _context.Update(entity);
    }

    public void DeleteAsync(T entity)
    {
        entity.DeletedDate = DateTime.Now;
        _context.Remove(entity);
    }

    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(a=>a.Id == id, cancellationToken);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<T>().ToListAsync(cancellationToken);
    }
}