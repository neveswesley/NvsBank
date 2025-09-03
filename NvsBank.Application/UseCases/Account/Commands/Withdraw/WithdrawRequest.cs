using MediatR;

namespace NvsBank.Application.UseCases.Account.Commands.Withdraw;

public sealed record WithdrawRequest (Guid Id, decimal Amount) : IRequest<WithdrawResponse>;