namespace NvsBank.Domain.Entities.DTO;

public class BankSlipResponse
{
    public Guid Id { get; set; }
    public string DigitableLine { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public Guid AccountPayeeId { get; set; }
    public Guid CustomerPayerId { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }
}