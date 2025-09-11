using Microsoft.EntityFrameworkCore;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class BankSlipRepository : IBankSlipRepository
{
    private readonly AppDbContext _context;

    public BankSlipRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BankSlip> AddAsync(BankSlip bankSlip)
    {
        await _context.BankSlips.AddAsync(bankSlip);
        return bankSlip;
    }

    public async Task<BankSlip> GetByDigitableLine(string digitableLine)
    {
        var bankSlip = await _context.BankSlips.FirstOrDefaultAsync(x=>x.DigitableLine == digitableLine);
        return bankSlip;
    }

    public async Task<BankSlip> GetByPayeeId(Guid payeeId)
    {
        var payee = await _context.BankSlips.FirstOrDefaultAsync(x=>x.AccuntPayeeId == payeeId);
        return payee;
    }

    public async Task<BankSlip> GetByPayerId(Guid payerId)
    {
        var payee = await _context.BankSlips.FirstOrDefaultAsync(x=>x.CustomerPayerId == payerId);
        return payee;
    }
}