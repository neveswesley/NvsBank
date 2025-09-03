using MediatR;

namespace NvsBank.Application.UseCases.Account.Queries.GetBalance;

public sealed record GetBalanceRequest (Guid Id) : IRequest<GetBalanceResponse>;