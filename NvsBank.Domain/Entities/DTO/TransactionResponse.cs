namespace NvsBank.Domain.Entities.DTO;

public class TransactionResponse 
{
    public Guid TransactionId { get; set; }
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public decimal NewBalance { get; set; }
    public decimal OldBalance { get; set; }
    public string TransactionType { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; }
}