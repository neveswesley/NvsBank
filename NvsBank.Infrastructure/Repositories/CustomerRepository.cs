using Microsoft.EntityFrameworkCore;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.Enums;
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

    public Task<Account> GetActiveCustomer(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Customer>> GetAllWithAddressAsync()
    {
        return await _context.Customers.Include(x => x.Address).Include(c=>c.Account).ToListAsync();
    }

    public async Task<Customer> GetByIdWithAddressAsync(Guid id)
    {
        return await _context.Customers.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Customer> GetByDocumentWithAddressAsync(string document)
    {
        return await _context.Customers.Include(x => x.Address).FirstOrDefaultAsync(x => x.DocumentNumber == document);
    }

    public async Task<Customer> DeleteAddress(Guid id)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        
        customer.Address = null;
        
        return customer;
    }

    public void InactiveAsync(Customer customer)
    {
        customer.DeletedDate = DateTime.Now;
        customer.CustomerStatus = CustomerStatus.Inactive;
        
        _context.Customers.Update(customer);
    }

    public void ActiveAsync(Customer customer)
    {
        customer.DeletedDate = DateTime.Now;
        customer.CustomerStatus = CustomerStatus.Active;
        
        _context.Customers.Update(customer);
    }

    public void SuspendAsync(Customer customer)
    {
        customer.DeletedDate = DateTime.Now;
        customer.CustomerStatus = CustomerStatus.Suspended;
        
        _context.Customers.Update(customer);
    }

    public void BlockAsync(Customer customer)
    {
        customer.DeletedDate = DateTime.Now;
        customer.CustomerStatus = CustomerStatus.Blocked;
        
        _context.Customers.Update(customer);
    }
    
    public void CloseAsync(Customer customer)
    {
        customer.DeletedDate = DateTime.Now;
        customer.CustomerStatus = CustomerStatus.Closed;
        
        _context.Customers.Update(customer);
    }
}