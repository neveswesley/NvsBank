using Microsoft.EntityFrameworkCore;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken)
    {
        await _context.Payments.AddAsync(payment, cancellationToken);
        return payment;
    }

    public async Task<Payment?> GetByIdempotencyKeyAsync(Guid bankSlipId, string idempotencyKey, CancellationToken cancellationToken)
    {
        return await _context.Payments.FirstOrDefaultAsync(p=> p.BankSlipId == bankSlipId && p.IdempotencyKey == idempotencyKey, cancellationToken);
    }
}