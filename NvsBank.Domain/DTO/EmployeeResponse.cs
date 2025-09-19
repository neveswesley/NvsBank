using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities.DTO;

public class EmployeeResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Guid UserId { get; set; }

    public PersonStatus Status { get; set; }

}