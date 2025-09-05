using MediatR;
using NvsBank.Application.UseCases.Transaction.Queries.GeAllTransactions;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Infrastructure.Repositories;

namespace NvsBank.Application.UseCases.Transaction.Queries.GetTransictionQuery;

public class GetTransactionQueryHandler : IRequestHandler<GetTransactionQueryRequest, List<TransactionResponse>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<List<TransactionResponse>> Handle(GetTransactionQueryRequest request,
        CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetByAccountIdAsync(request.AccountId);

        return transactions.Select(t => new TransactionResponse
        {
            TransactionId = t.Id,
            AccountId = t.AccountId,
            Amount = t.Amount,
            NewBalance = t.NewBalance,
            OldBalance = t.OldBalance,
            TransactionType = t.TransactionType.ToString(),
            Description = t.Description,
            Timestamp = t.Timestamp,
        }).ToList();
    }
}