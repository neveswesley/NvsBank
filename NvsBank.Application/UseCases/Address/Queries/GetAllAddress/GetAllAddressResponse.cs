namespace NvsBank.Application.UseCases.Address.Queries.GetAllAddress;

public sealed record GetAllAddressResponse
{
    public Guid Id { get; set; }
    public string Street { get; set; } = String.Empty;
    public string Number { get; set; } = String.Empty;
    public string City { get; set; } = String.Empty;
    public string State { get; set; } = String.Empty;
    public string ZipCode { get; set; } = String.Empty;
    
    public string CustomerName { get; set; }
    
    public GetAllAddressResponse()
    {
        
    }
}