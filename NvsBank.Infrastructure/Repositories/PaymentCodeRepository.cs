using Microsoft.EntityFrameworkCore;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class PaymentCodeRepository : IPaymentCodeRepository
{
    
    private readonly AppDbContext _context;

    public PaymentCodeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentCode?> GetByPaymentCode(string paymentCode)
    {
        var receipt = await _context.PaymentCodes.FirstOrDefaultAsync(x=>x.RandomPaymentCode == paymentCode);
        return receipt;
    }

    public async Task<PaymentCode> CreateAsync(PaymentCode paymentCode)
    {
        await _context.PaymentCodes.AddAsync(paymentCode);
        return paymentCode;
    }

    public async Task<List<PaymentCode?>> GetAllAsync()
    {
        var receipts = await _context.PaymentCodes.ToListAsync();
        return receipts;
    }
}