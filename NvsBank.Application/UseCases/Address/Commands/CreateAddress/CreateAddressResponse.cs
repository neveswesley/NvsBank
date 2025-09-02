namespace NvsBank.Application.UseCases.Address.Commands.CreateAddress;

public class CreateAddressResponse
{
    public string Street { get; set; }
    public string Number { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    
    public string CustomerName { get; set; }
}
