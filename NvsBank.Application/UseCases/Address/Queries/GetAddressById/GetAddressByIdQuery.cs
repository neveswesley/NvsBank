using MediatR;

namespace NvsBank.Application.UseCases.Address.Queries.GetAddressById;

public sealed record GetAddressByIdQuery(Guid Id) : IRequest<GetAddressByIdResponse>;