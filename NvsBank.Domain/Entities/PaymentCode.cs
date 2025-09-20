namespace NvsBank.Domain.Entities;

public class PaymentCode
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string RandomPaymentCode { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Created { get; set; }
    public bool IsPaid { get; set; } = false;
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(1);
}