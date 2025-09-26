using NvsBank.Application.Shared.Extras;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities;

public class Account : BaseEntity
{
    public string AccountNumber { get; set; } = BankAccountGenerator.GenerateAccountNumber();
    public string Branch { get; set; } = BankAccountGenerator.GenerateBranchNumber();
    public AccountType AccountType { get; set; }
    public decimal Balance { get; set; } = decimal.Zero;
    public decimal OverdraftLimit { get; set; } = decimal.Zero;
    public DateTime OpeningDate { get; set; } = DateTime.Today;
    public DateTime? ClosingDate { get; set; }
    public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;
    public string? StatusReason { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public IEnumerable<PixArea>? PixKey { get; set; }
    public void Deposit(decimal amount)
    {
        if (amount < 0)
            throw new InvalidOperationException($"Cannot add balance because amount is negative: {amount}");
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (Balance < amount)
            throw new InvalidOperationException($"The account {AccountNumber} does not have enough balance");
        
        Balance -= amount;
    }

    public decimal GetBalance()
    {
        return Balance;
    }

}