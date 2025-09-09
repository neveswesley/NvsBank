using Microsoft.EntityFrameworkCore;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext Context;

    protected BaseRepository(AppDbContext context)
    {
        Context = context;
    }


    public async Task<T> CreateAsync(T entity)
    {
        entity.CreatedDate = DateTime.Now;
        await Context.AddAsync(entity);
        return entity;
    }

    public void UpdateAsync(T entity)
    {
        entity.ModifiedDate = DateTime.Now;
        Context.Update(entity);
    }

    public void DeleteAsync(T entity)
    {
        entity.DeletedDate = DateTime.Now;
        Context.Remove(entity);
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await Context.Set<T>().FirstOrDefaultAsync(a=>a.Id == id);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<T>().ToListAsync(cancellationToken);
    }
}