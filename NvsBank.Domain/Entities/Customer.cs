using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities;

public sealed class Customer : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public CustomerType Type { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public DateTime? FoundationDate { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public CustomerStatus CustomerStatus { get; set; }
    public string? StatusReason { get; set; }
    
    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }
    public Guid? AccountId { get; set; }
    public IEnumerable<Account>? Account { get; set; }

    public void UpdateCustomer(string fullName, string phoneNumber, string email)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
    }
}