using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities;

public class Employee : Person
{
    public EmployeeType Type { get; set; }

    public void CompleteRegistrationEmployee(string documentNumber, DateTime birthDate, string phoneNumber)
    {
        if (DocumentNumber != null)
            throw new ApplicationException("DocumentNumber already exists.");
        DocumentNumber = documentNumber;

        if (BirthDate != null)
            throw new ApplicationException("BirthDate already exists.");
        BirthDate = birthDate;

        if (PhoneNumber != null)
            throw new ApplicationException("PhoneNumber already exists.");
        PhoneNumber = phoneNumber;
    }
}