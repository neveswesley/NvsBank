using MediatR;
using NvsBank.Application.UseCases.Address.Queries.GetAllAddress;

namespace NvsBank.Application.UseCases.Address.Queries;

public sealed record GetAllAddressQuery : IRequest<List<GetAllAddressResponse>>
{
    
}