using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities;

public abstract class Person : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = string.Empty;
    public PersonStatus Status { get; set; }
    public string? StatusReason { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public OperationLimits Limits { get; set; } = new();


}