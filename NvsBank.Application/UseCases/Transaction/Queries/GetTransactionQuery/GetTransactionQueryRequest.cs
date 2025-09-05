using MediatR;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Transaction.Queries.GeAllTransactions;

public sealed record GetTransactionQueryRequest (Guid AccountId) : IRequest<List<TransactionResponse>>;