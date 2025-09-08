using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Infrastructure.Repositories;

namespace NvsBank.Application.UseCases.Transaction.Commands;

public abstract class Withdraw
{
    public sealed record WithdrawCommand (Guid Id, decimal Amount, string Description) : IRequest<TransactionResponse>;
    
    public class WithdrawHandler : IRequestHandler<WithdrawCommand, TransactionResponse>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WithdrawHandler(IAccountRepository accountRepository, ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionResponse> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.Id);
        if (account == null) throw new ApplicationException($"Account {request.Id} not found");

        if (account.Balance < request.Amount)
            throw new ApplicationException($"Account {request.Id} has insufficient balance for this withdrawal");

        account.Balance -= request.Amount;

        _accountRepository.UpdateAsync(account);


        var transaction = new Domain.Entities.Transaction
        {
            AccountId = account.Id,
            NewBalance = account.Balance,
            Amount = request.Amount,
            OldBalance = request.Amount + request.Amount,
            TransactionType = TransactionType.Withdraw,
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
            OldBalance = account.Balance + request.Amount,
            TransactionType = TransactionType.Withdraw.ToString(),
            Description = transaction.Description,
            Timestamp = transaction.Timestamp
        };
    }
}
}