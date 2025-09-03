using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Account.Commands.CreateAccount;

public sealed record CreateAccountResponse
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; }
    public string Branch { get; set; }
    public AccountType AccountType { get; set; }
    public decimal Balance { get; set; }
    public decimal? OverdraftLimit { get; set; }
    public DateTime OpeningDate { get; set; }
    public AccountStatus Status { get; set; }
    public Guid CustomerId { get; set; }
}