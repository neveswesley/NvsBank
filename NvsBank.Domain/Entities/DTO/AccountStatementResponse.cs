namespace NvsBank.Domain.Entities.DTO;

public class AccountStatementResponse
{
    public Guid AccountId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<StatementItem> Transactions { get; set; }
    public decimal FinalBalance { get; set; }
}