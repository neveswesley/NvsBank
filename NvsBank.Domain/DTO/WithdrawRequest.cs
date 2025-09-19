namespace NvsBank.Domain.Entities.DTO;

public class WithdrawRequest
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
}