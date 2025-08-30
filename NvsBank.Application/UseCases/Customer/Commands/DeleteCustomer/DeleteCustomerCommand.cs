using MediatR;

namespace NvsBank.Application.UseCases.Customer.Commands.DeleteCustomer;

public sealed record DeleteCustomerCommand(Guid Id) : IRequest<DeleteCustomerResponse>;