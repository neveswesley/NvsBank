using MediatR;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Customer.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<GetCustomerByIdResponse>;