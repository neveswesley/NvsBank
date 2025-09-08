using System.ComponentModel;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public decimal NewBalance { get; set; }
    public decimal OldBalance { get; set; }
    public TransactionType TransactionType { get; set; }
    public string Description { get; set; } = String.Empty;
    public DateTime Timestamp { get; set; }

    public Transaction()
    {
        
    }
}