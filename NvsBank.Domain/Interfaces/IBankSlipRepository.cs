using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Domain.Interfaces;

public interface IBankSlipRepository
{
    Task<BankSlip> AddAsync(BankSlip bankSlip); 
    Task<BankSlip> GetByDigitableLine(string digitableLine); 
    Task<PagedResult<BankSlip>> GetAllBankSlipByStatus(bool status, int page, int pageSize); 
    Task<PagedResult<BankSlip>> GetByPayeeId(Guid payeeId, int page, int pageSize); 
    Task<PagedResult<BankSlip>> GetByPayerId(Guid payerId, int page, int pageSize); 
}