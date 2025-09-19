namespace NvsBank.Domain.Entities.DTO;

public class AddressResponse
{
    public Guid Id { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public Guid CustomerId { get; set; }

    public AddressResponse(Guid id, string street, string number, string city, string state, string zipCode,
        Guid customerId)
    {
        Id = id;
        Street = street;
        Number = number;
        City = city;
        State = state;
        ZipCode = zipCode;
        CustomerId = customerId;
    }

    public AddressResponse()
    {
        
    }
}