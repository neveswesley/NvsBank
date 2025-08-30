using MediatR;

namespace NvsBank.Application.UseCases.Customer.Queries.GetAllCustomer;

public sealed record GetAllCustomerQuery : IRequest<List<GetAllCustomerResponse>>
{
    
}