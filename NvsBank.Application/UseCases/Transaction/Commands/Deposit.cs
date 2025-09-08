using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Infrastructure.Repositories;

namespace NvsBank.Application.UseCases.Transaction.Commands;

public abstract class Deposit
{
    public sealed record DepositCommand(Guid Id, decimal Amount, string Description) : IRequest<TransactionResponse>;

    public class DepositHandler : IRequestHandler<DepositCommand, TransactionResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepositHandler(IAccountRepository accountRepository, ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionResponse> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.Id);
            if (account == null) throw new ApplicationException("Account not found");

            account.Balance += request.Amount;
            _accountRepository.UpdateAsync(account);

            var transaction = new Domain.Entities.Transaction
            {
                AccountId = account.Id,
                NewBalance = account.Balance,
                OldBalance = account.Balance - request.Amount,
                Amount = request.Amount,
                TransactionType = TransactionType.Deposit,
                Description = request.Description
            };

            await _transactionRepository.AddAsync(transaction);

            await _unitOfWork.Commit(cancellationToken);

            return new TransactionResponse
            {
                TransactionId = transaction.Id,
                AccountId = transaction.AccountId,
                Amount = transaction.Amount,
                NewBalance = account.Balance,
                OldBalance = account.Balance - transaction.Amount,
                TransactionType = transaction.TransactionType.ToString(),
                Description = transaction.Description,
                Timestamp = transaction.Timestamp
            };
        }
    }
}