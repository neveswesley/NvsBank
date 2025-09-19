namespace NvsBank.Domain.Entities.DTO;

public class DepositRequest
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
}