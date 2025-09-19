using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities.DTO;

public class GetCustomerResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public CustomerType? Type { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public PersonStatus CustomerStatus { get; set; }
    
    public string? AddressStreet { get; set; }
    public string? AddressNumber { get; set; }
    public string? AddressCity { get; set; }
    public string? AddressState { get; set; }
    public string? AddressZipCode { get; set; }
    public IEnumerable<AccountResponse> Accounts { get; set; } = new List<AccountResponse>();
}