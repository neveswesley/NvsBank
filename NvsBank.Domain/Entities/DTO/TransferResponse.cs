using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities.DTO;

public class TransferResponse
{
    public Guid FromAccountId { get; set; }
    public Guid ToAcountId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; }
}