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
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WithdrawHandler(IAccountRepository accountRepository, ITransactionRepository transactionRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionResponse> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null) throw new ApplicationException($"Account {request.Id} not found");

        if (account.AccountStatus != AccountStatus.Active)
            throw new ApplicationException($"Account {request.Id} is not active");

        var customer = await _customerRepository.GetByIdAsync(account.CustomerId);
        
        if (customer.CustomerStatus != CustomerStatus.Active)
            throw new ApplicationException($"Customer {request.Id} is not active");
        
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