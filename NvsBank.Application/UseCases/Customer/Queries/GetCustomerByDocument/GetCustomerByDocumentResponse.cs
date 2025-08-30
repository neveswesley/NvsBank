using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Customer.Queries.GetCustomerByDocument;

public sealed record GetCustomerByDocumentResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public CustomerType Type { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public DateTime? FoundationDate { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public CustomerStatus CustomerStatus { get; set; }

    public GetCustomerByDocumentResponse()
    {
        
    }
}