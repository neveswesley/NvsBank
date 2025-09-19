using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities.DTO;

public class CustomerStatusResponse
{
    public Guid CustomerId { get; set; }
    public PersonStatus OldStatus { get; set; }
    public PersonStatus NewStatus { get; set; }
    public string? Reason { get; set; }
    public DateTime ChangedAt { get; set; }
}