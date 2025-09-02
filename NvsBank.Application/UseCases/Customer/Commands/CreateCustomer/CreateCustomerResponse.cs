using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Employee.Commands.CreateEmployee;

public class CreateCustomerResponse
{
    public string FullName { get; set; } = string.Empty;
    public CustomerType Type { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public DateTime? FoundationDate { get; set; }
    public Domain.Entities.Address? Address { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public CustomerStatus CustomerStatus { get; set; }
}