using MediatR;
using NvsBank.Application.UseCases.Address.Commands.UpdateAddress;

namespace NvsBank.Application.UseCases.Address.Commands.DeleteAddress;

public sealed record DeleteAddressCommand (Guid Id) : IRequest<DeleteAddressResponse>;