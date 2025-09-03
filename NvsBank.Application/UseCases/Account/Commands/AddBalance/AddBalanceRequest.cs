using MediatR;

namespace NvsBank.Application.UseCases.Account.Commands.AddBalance;

public sealed record AddBalanceRequest(Guid Id, decimal Amount) : IRequest<AddBalanceResponse>;