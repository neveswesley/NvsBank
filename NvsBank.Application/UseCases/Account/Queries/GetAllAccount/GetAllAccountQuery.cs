using MediatR;

namespace NvsBank.Application.UseCases.Account.Queries.GetAllAccount;

public sealed record GetAllAccountQuery : IRequest<List<GetAllAccountResponse>>;