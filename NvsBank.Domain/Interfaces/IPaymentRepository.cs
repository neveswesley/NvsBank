using NvsBank.Domain.Entities;

namespace NvsBank.Domain.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> AddAsync(Payment payment, CancellationToken cancellationToken);
    Task<Payment?> GetByIdempotencyKeyAsync(Guid bankSlipId, string idempotencyKey, CancellationToken cancellationToken);
}