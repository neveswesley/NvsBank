using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> CreateAsync(T entity);
    void UpdateAsync(T entity);
    void DeleteAsync(T entity);
    Task<T> GetByIdAsync(Guid id);
    Task<PagedResult<T>> GetPagedAsync(int page, int pageSize);

}