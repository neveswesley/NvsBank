using Microsoft.EntityFrameworkCore;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Customer> _dbSet;

    public CustomerRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Customer>();
    }

    public async Task<Customer> GetByDocument(string document)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.DocumentNumber == document);
        if (customer == null)
            throw new KeyNotFoundException();
        
        return customer;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Customers.AnyAsync(x => x.Email == email);
    }

    public Task<Account> GetActiveCustomer(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResult<Customer>> GetAllWithAddressAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        
        var totalCount = await _dbSet.CountAsync();

        var items = await _dbSet.Include(x=>x.Address).Include(x=>x.Accounts).OrderBy(x=>x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Customer>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Customer> GetByIdWithAddressAsync(Guid id)
    {
        var customer = await _context.Customers.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
        if (customer == null)
            throw new KeyNotFoundException();
        
        return customer;
    }

    public async Task<Customer> GetByDocumentWithAddressAsync(string document)
    {
        var customer = await _context.Customers.Include(x => x.Address).FirstOrDefaultAsync(x => x.DocumentNumber == document);
        if (customer == null)
            throw new KeyNotFoundException("Customer not found");
        
        return customer;
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
        customer.Status = PersonStatus.Inactive;
        
        _context.Customers.Update(customer);
    }

    public void ActiveAsync(Customer customer)
    {
        customer.Status = PersonStatus.Active;
        
        _context.Customers.Update(customer);
    }

    public void SuspendAsync(Customer customer)
    {
        customer.DeletedDate = DateTime.Now;
        customer.Status = PersonStatus.Suspended;
        
        _context.Customers.Update(customer);
    }

    public void BlockAsync(Customer customer)
    {
        customer.DeletedDate = DateTime.Now;
        customer.Status = PersonStatus.Blocked;
        
        _context.Customers.Update(customer);
    }
    
    public void CloseAsync(Customer customer)
    {
        customer.DeletedDate = DateTime.Now;
        customer.Status = PersonStatus.Closed;
        
        _context.Customers.Update(customer);
    }
}