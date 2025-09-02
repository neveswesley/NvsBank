namespace NvsBank.Application.UseCases.Address.Queries.GetAddressById;

public sealed record GetAddressByIdResponse
{
    public Guid Id { get; set; }
    public string Street { get; set; } = String.Empty;
    public string Number { get; set; } = String.Empty;
    public string City { get; set; } = String.Empty;
    public string State { get; set; } = String.Empty;
    public string ZipCode { get; set; } = String.Empty;
    
    public string CustomerName { get; set; }

    public GetAddressByIdResponse()
    {
        
    }
}