using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities.DTO;

public class CustomerResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public CustomerType CustomerType { get; set; }
    public string DocumentNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? FoudationDate { get; set; }

    public DateTime? RelevanteDate
    {
        get
        {
            return CustomerType == CustomerType.Individual ? FoudationDate : BirthDate;
        }
    }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public CustomerStatus CustomerStatus { get; set; }

}