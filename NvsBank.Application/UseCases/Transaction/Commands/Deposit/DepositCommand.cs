using MediatR;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Transaction.Commands.Deposit;

public sealed record DepositCommand(Guid Id, decimal Amount, string Description) : IRequest<TransactionResponse>;