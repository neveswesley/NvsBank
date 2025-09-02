namespace NvsBank.Application.UseCases.Address.Commands.UpdateAddress;

public sealed record UpdateAddressResponse(
    Guid Id,
    string Street,
    string Number,
    string City,
    string State,
    string ZipCode,
    Guid? CustomerId);