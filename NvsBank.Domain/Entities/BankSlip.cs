namespace NvsBank.Domain.Entities;

public class BankSlip
{
    public Guid Id { get; set; }
    public string DigitableLine { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(3);
    private bool AllowAfterDueDate { get; set; } = true;
    public Guid AccountPayeeId { get; set; }
    public Guid CustomerPayerId { get; set; }
    public bool IsPaid { get; set; } = false;
    public DateTime? PaidAt { get; set; }

    
    public decimal CalculatePayableAmount(DateTime paymentDate)
    {
        if (paymentDate <= DueDate)
            return Amount;
        
        if (!AllowAfterDueDate)
            throw new ApplicationException("Payment date cannot be after DueDate");

        var daysLate = (paymentDate.Date - DueDate.Date).Days;
        var penalty = Amount * 0.02m; // multa fixa em 2%
        var interest = Amount * 0.01m * daysLate; // juros de 1% ao dia
        
        return Amount + penalty + interest;
    }

    public void MarkAsPaid()
    {
        IsPaid = true;
        PaidAt = DateTime.Now;
    }
    
}