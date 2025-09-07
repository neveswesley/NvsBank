namespace NvsBank.Domain.Entities;

public class BankSlip
{
    public Guid Id { get; set; }
    public string DigitableLine { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(3);
    public Guid PayeeId { get; set; }
    public Guid PayerId { get; set; }
    public bool IsPaid { get; set; } = false;
    public DateTime? PaidAt { get; set; }
}