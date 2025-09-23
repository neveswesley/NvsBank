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
            throw new ApplicationException("Customer has already been created.");
        CustomerType = type;
        
        if (DocumentNumber != null)
            throw new ApplicationException("Customer has already been created.");
        DocumentNumber = documentNumber;
        
        if (BirthDate != null)
            throw new ApplicationException("Customer has already been created.");
        BirthDate = birthDate;
        
        if (PhoneNumber != null)
            throw new ApplicationException("Customer has already been created.");
        PhoneNumber = phoneNumber;
        
    }
    public bool IsActive(PersonStatus status)
    {
        
        if (status is PersonStatus.Pending or PersonStatus.Blocked or PersonStatus.Closed or PersonStatus.Inactive or PersonStatus.Suspended)
        {
            return false;
            
        }
        
        return true;
    }
    
}
