using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Domain.Interfaces;

public interface IBankSlipRepository
{
    Task<BankSlip> AddAsync(BankSlip bankSlip); 
    Task<BankSlip> GetByDigitableLine(string digitableLine); 
    Task<PagedResult<BankSlip>> GetAllBankSlipByStatus(bool status, int page, int pageSize); 
    Task<BankSlip> GetByPayeeId(Guid payeeId); 
    Task<BankSlip> GetByPayerId(Guid payerId); 
}