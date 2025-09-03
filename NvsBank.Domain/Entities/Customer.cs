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
    
    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }
    public Guid? AccountId { get; set; }
    public IEnumerable<Account>? Account { get; set; }

    public void UpdateCustomer(string fullName, CustomerType type, string documentNumber, DateTime? birthDate, DateTime? foundationDate, string phoneNumber, string email, CustomerStatus status)
    {
        FullName = fullName;
        Type = type;
        DocumentNumber = documentNumber;
        BirthDate = birthDate;
        FoundationDate = foundationDate;
        PhoneNumber = phoneNumber;
        Email = email;
        CustomerStatus = status;
    }
}