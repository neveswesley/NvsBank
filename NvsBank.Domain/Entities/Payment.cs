namespace NvsBank.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid BankSlipId { get; set; }
    public Guid? PayerAccountId { get; set; }
    public Guid? PayeeAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    public string IdempotencyKey { get; set; } = string.Empty;
    
}