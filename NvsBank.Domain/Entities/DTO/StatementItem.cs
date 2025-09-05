using System.Text.Json.Serialization;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities.DTO;

public class StatementItem
{
    public Guid TransactionId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionType  Type { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceAfterTransaction { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; }
}