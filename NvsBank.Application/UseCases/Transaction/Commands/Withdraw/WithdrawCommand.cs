using MediatR;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Account.Commands.Withdraw;

public sealed record WithdrawCommand (Guid Id, decimal Amount, string Description) : IRequest<TransactionResponse>;