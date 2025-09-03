using MediatR;
using NvsBank.Application.Shared.Extras;
using NvsBank.Application.UseCases.Account.Commands.CreateAccount;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Account.CreateAccount;

public sealed record CreateAccountCommand : IRequest<CreateAccountResponse>
{
    public AccountType AccountType { get; set; }
    public Guid CustomerId { get; set; }
}