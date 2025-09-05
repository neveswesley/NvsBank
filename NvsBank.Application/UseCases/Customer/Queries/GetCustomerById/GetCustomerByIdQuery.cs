using MediatR;
using NvsBank.Application.UseCases.Customer.Queries.GetAllCustomer;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Customer.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<GetAllCustomerResponse>;