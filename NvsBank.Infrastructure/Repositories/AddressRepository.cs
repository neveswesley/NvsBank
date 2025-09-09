using Microsoft.EntityFrameworkCore;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class AddressRepository : BaseRepository<Address>, IAddressRepository
{
    public AddressRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<List<Address>> GetAllWithCustomerAsync()
    {
        return await Context.Addresses
            .Include(a => a.Customer)
            .ToListAsync();
    }

    public async Task<Address> GetByIdWithCustomerAsync(Guid id)
    {
        return await Context.Addresses.Include(a => a.Customer).FirstOrDefaultAsync(x => x.Id == id);
    }
}