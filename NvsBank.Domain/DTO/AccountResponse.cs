using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities.DTO;

public record AccountResponse
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
}