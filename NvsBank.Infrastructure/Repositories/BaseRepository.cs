using Microsoft.EntityFrameworkCore;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> DbSet;

    protected BaseRepository(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
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
    public async Task<PagedResult<T>> GetPagedAsync(int page = 1, int pageSize = 10)
    {
        var totalCount = await DbSet.CountAsync();
        
        var items = await DbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}