using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Account.Queries.GetAllAccount;

public sealed record GetAllAccountResponse
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; }
    public string Branch { get; set; }
    public AccountType AccountType { get; set; }
    public decimal Balance { get; set; }
    public decimal OverdraftLimit { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public AccountStatus Status { get; set; }
    public Guid CustomerId { get; set; }

    public GetAllAccountResponse()
    {
        
    }
}