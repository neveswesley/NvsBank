using MediatR;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Employee.Commands.CreateEmployee;

public sealed record CreateCustomerCommand : IRequest<CreateCustomerResponse>
{
    public string FullName { get; set; } = string.Empty;
    public CustomerType Type { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public DateTime? FoundationDate { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public CustomerStatus CustomerStatus { get; set; }
}