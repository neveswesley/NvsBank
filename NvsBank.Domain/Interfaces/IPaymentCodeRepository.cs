using NvsBank.Domain.Entities;

namespace NvsBank.Domain.Interfaces;

public interface IPaymentCodeRepository
{
    Task<PaymentCode?> GetByPaymentCode(string paymentCode);
    Task<PaymentCode> CreateAsync(PaymentCode paymentCode);
    Task<List<PaymentCode?>> GetAllAsync();
}