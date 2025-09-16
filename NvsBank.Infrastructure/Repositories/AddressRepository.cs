using Microsoft.EntityFrameworkCore;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class AddressRepository : BaseRepository<Address>, IAddressRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Address> _dbSet;

    public AddressRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Addresses;
    }

    public async Task<PagedResult<Address>> GetAllWithCustomerAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        
        var totalCount = await _dbSet.CountAsync();

        var items = await _dbSet.Include(a=>a.Customer).OrderBy(x=>x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Address>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Address> GetByIdWithCustomerAsync(Guid id)
    {
        return await Context.Addresses.Include(a => a.Customer).FirstOrDefaultAsync(x => x.Id == id);
    }
}