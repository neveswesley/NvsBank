using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities.DTO;

public class CustomerResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public CustomerType? Type { get; set; }
    public string DocumentNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public PersonStatus Status { get; set; }

}