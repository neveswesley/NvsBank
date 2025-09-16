using Microsoft.EntityFrameworkCore;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class BankSlipRepository : IBankSlipRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<BankSlip> _dbSet;

    public BankSlipRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<BankSlip>();
    }

    public async Task<BankSlip> AddAsync(BankSlip bankSlip)
    {
        await _context.BankSlips.AddAsync(bankSlip);
        return bankSlip;
    }

    public async Task<BankSlip> GetByDigitableLine(string digitableLine)
    {
        var bankSlip = await _context.BankSlips.FirstOrDefaultAsync(x=>x.DigitableLine == digitableLine);
        if (bankSlip == null)
            throw new ApplicationException("Bank slip not found");
        return bankSlip;
    }

    public async Task<PagedResult<BankSlip>> GetAllBankSlipByStatus(bool status, int page, int pageSize)
    {
        
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        
        var totalCount = await _dbSet.Where(x=>x.IsPaid == status).CountAsync();

        var items = await _dbSet.Where(x=>x.IsPaid == status).OrderBy(x=>x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<BankSlip>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<BankSlip> GetByPayeeId(Guid payeeId)
    {
        var payee = await _context.BankSlips.FirstOrDefaultAsync(x=>x.AccountPayeeId == payeeId);
        if (payee == null)
            throw new ApplicationException("Payee not found");
        return payee;
    }

    public async Task<BankSlip> GetByPayerId(Guid payerId)
    {
        var payer = await _context.BankSlips.FirstOrDefaultAsync(x=>x.CustomerPayerId == payerId);
        if (payer == null)
            throw new ApplicationException("Payer not found");
        return payer;
    }
}