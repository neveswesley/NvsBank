using NvsBank.Domain.Entities;

namespace NvsBank.Domain.Interfaces;

public interface IBankSlipRepository
{
    Task<BankSlip> AddAsync(BankSlip bankSlip); 
    Task<BankSlip> GetByDigitableLine(string digitableLine); 
    Task<BankSlip> GetByPayeeId(Guid payeeId); 
    Task<BankSlip> GetByPayerId(Guid payerId); 
}