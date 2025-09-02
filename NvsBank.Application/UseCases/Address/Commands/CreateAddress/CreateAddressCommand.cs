using MediatR;

namespace NvsBank.Application.UseCases.Address.Commands.CreateAddress;

public sealed record CreateAddressCommand : IRequest<CreateAddressResponse>
{
    public string Street { get; set; }
    public string Number { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public Guid? CustomerId { get; set; }
}