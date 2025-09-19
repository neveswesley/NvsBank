using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities.DTO;

public class PixTransferResponse
{
    public Guid FromAccountId { get; set; }
    public string ToPixKey { get; set; } = String.Empty;
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
    public string? Description { get; set; }
    public DateTime Timestamp { get; set; }
}