using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities;

public sealed class Customer : Person
{
    public CustomerType? CustomerType { get; set; }
    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }
    public Guid AccountId { get; set; }
    public ICollection<Account> Accounts { get; set; } = new List<Account>();

    public void UpdateCustomer(string fullName, string phoneNumber, string email)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public void CompleteRegistration(CustomerType type, string documentNumber, DateTime birthDate, string phoneNumber)
    {
        if (CustomerType != null)
            throw new ApplicationException("CustomerType already exists.");
        CustomerType = type;
        
        if (DocumentNumber != null)
            throw new ApplicationException("CustomerType already exists.");
        DocumentNumber = documentNumber;
        
        if (BirthDate != null)
            throw new ApplicationException("CustomerType already exists.");
        BirthDate = birthDate;
        
        if (PhoneNumber != null)
            throw new ApplicationException("CustomerType already exists.");
        PhoneNumber = phoneNumber;
        
    }
    
}
