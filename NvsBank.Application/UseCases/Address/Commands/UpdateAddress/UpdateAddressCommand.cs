using MediatR;
using NvsBank.Application.UseCases.Address.Queries.GetAddressById;

namespace NvsBank.Application.UseCases.Address.Commands.UpdateAddress;

public sealed record UpdateAddressCommand(
    Guid Id,
    string Street,
    string Number,
    string City,
    string State,
    string ZipCode) : IRequest<UpdateAddressResponse>;