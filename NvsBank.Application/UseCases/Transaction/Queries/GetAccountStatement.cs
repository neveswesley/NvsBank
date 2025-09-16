using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Infrastructure.Repositories;

namespace NvsBank.Application.UseCases.Transaction.Queries;

public abstract class GetAccountStatement
{
    public sealed record GetAccountStatementQuery(Guid AccountId, int From, int Page, int PageSize) : IRequest<AccountStatementResponse>;

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
            var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
            if (account == null)
                throw new ApplicationException($"Account {request.AccountId} not found");

            var allTransactions = await _transactionRepository.GetByAccountIdAsync(account.Id, request.Page, request.PageSize);

            var fromDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(request.From));

            decimal GetSignedAmount(Domain.Entities.Transaction t) =>
                t.TransactionType == TransactionType.Withdraw ? -t.Amount : t.Amount;
            
            decimal runningBalance = account.Balance;
            foreach (var t in allTransactions.Items.Where(t => t.Timestamp < fromDate))
            {
                runningBalance += GetSignedAmount(t);
            }
            
            var transactions = allTransactions
                .Items.Where(t => t.Timestamp >= fromDate)
                .OrderBy(t => t.Timestamp)
                .ToList();

            var statementItems = new List<StatementItem>();
            foreach (var t in transactions)
            {
                runningBalance += GetSignedAmount(t);

                statementItems.Add(new StatementItem
                {
                    TransactionId = t.Id,
                    Type = t.TransactionType,
                    Amount = GetSignedAmount(t),
                    BalanceAfterTransaction = runningBalance,
                    Description = t.Description,
                    Timestamp = t.Timestamp
                });
            }

            return new AccountStatementResponse
            {
                AccountId = account.Id,
                From = fromDate,
                Transactions = statementItems,
                FinalBalance = runningBalance
            };
        }
    }
}