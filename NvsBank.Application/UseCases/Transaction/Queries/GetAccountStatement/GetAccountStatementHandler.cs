using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Infrastructure.Repositories;

namespace NvsBank.Application.UseCases.Transaction.Queries.GetAccountSatement;

public class GetAccountStatementHandler : IRequestHandler<GetAccountStatementQuery, AccountStatementResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;

    public GetAccountStatementHandler(ITransactionRepository transactionRepository,
        IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }

    public async Task<AccountStatementResponse> Handle(GetAccountStatementQuery request,
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId);
        if (account == null)
            throw new ApplicationException($"Account {request.AccountId} not found");

        var allTransactions = await _transactionRepository.GetByAccountIdAsync(account.Id);

        var fromDate = request.From.Date;
        var toDateInclusive = request.To.Date.AddDays(1).AddTicks(-1);

        var transactions = allTransactions
            .Where(t => t.Timestamp >= fromDate && t.Timestamp <= toDateInclusive)
            .OrderBy(t => t.Timestamp)
            .ToList();

        decimal runningBalance = account.Balance;
        foreach (var t in allTransactions.Where(t => t.Timestamp > toDateInclusive))
        {
            runningBalance -= t.TransactionType == TransactionType.Withdraw ? -t.Amount : t.Amount;
        }

        var statementItems = new List<StatementItem>();
        foreach (var t in transactions)
        {
            runningBalance += t.TransactionType == TransactionType.Withdraw ? -t.Amount : t.Amount;
            statementItems.Add(new StatementItem
            {
                TransactionId = t.Id,
                Type = t.TransactionType,
                Amount = t.TransactionType == TransactionType.Withdraw ? -t.Amount : t.Amount,
                BalanceAfterTransaction = runningBalance,
                Description = t.Description,
                Timestamp = t.Timestamp
            });
        }

        return new AccountStatementResponse
        {
            AccountId = account.Id,
            From = request.From,
            To = request.To,
            Transactions = statementItems,
            FinalBalance = runningBalance
        };
    }
}