using Microsoft.EntityFrameworkCore;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Customer> GetByDocument(string document)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.DocumentNumber == document);
        return customer;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Customers.AnyAsync(x => x.Email == email);
    }

    public async Task<Customer> GetByIdAsync(Guid id)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Customer>> GetAllWithAddressAsync()
    {
        return await _context.Customers.Include(x => x.Address).ToListAsync();
    }

    public async Task<Customer> GetByIdWithAddressAsync(Guid id)
    {
        return await _context.Customers.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Customer> GetByDocumentWithAddressAsync(string document)
    {
        return await _context.Customers.Include(x => x.Address).FirstOrDefaultAsync(x => x.DocumentNumber == document);
    }
}