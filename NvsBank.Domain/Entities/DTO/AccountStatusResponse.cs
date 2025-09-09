using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities.DTO;

public class AccountStatusResponse
{
    public Guid AccountId { get; set; }
    public AccountStatus OldStatus { get; set; }
    public AccountStatus NewStatus { get; set; }
    public string? Reason { get; set; }
    public DateTime ChangeAt { get; set; }
}