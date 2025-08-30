using MediatR;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Customer.Commands.UpdateCustomer;

public sealed record UpdateCustomerResponse(
    Guid Id,
    string FullName,
    CustomerType Type,
    string DocumentNumber,
    DateTime? BirthDate,
    DateTime? FoundationDate,
    string PhoneNumber,
    string Email,
    CustomerStatus CustomerStatus);
