using MediatR;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Transaction.Queries.GetAccountSatement;

public sealed record GetAccountStatementQuery(Guid AccountId, DateTime From, DateTime To) : IRequest<AccountStatementResponse>;