using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Application.UseCases.Transaction.Commands.Deposit;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Infrastructure.Repositories;

namespace NvsBank.Application.UseCases.Transaction.Commands.Transfer;

public class TransferHandler : IRequestHandler<TransferCommand, TransferResponse>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransferHandler(IAccountRepository accountRepository, ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransferResponse> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var inAccount = await _accountRepository.GetByIdAsync(request.To);
        if (inAccount == null)
            throw new ApplicationException("Destination account not found.");

        var outAccount = await _accountRepository.GetByIdAsync(request.From);
        if (outAccount == null)
            throw new ApplicationException("Source account not found.");
        
        if (outAccount.Balance < request.Amount)
            throw new ApplicationException("Insufficient balance.");

        outAccount.Balance -= request.Amount;
        inAccount.Balance += request.Amount;

        _accountRepository.UpdateAsync(inAccount);
        _accountRepository.UpdateAsync(outAccount);

        var fromAccount = new Domain.Entities.Transaction
        {
            AccountId = outAccount.Id,
            NewBalance = outAccount.Balance,
            OldBalance = outAccount.Balance + request.Amount,
            Amount = request.Amount,
            TransactionType = TransactionType.Transfer,
            Description = request.Description
        };

        await _transactionRepository.AddAsync(fromAccount);

        var toAccount = new Domain.Entities.Transaction
        {
            AccountId = inAccount.Id,
            NewBalance = inAccount.Balance,
            OldBalance = inAccount.Balance - request.Amount,
            Amount = request.Amount,
            TransactionType = TransactionType.Transfer,
            Description = request.Description
        };

        await _transactionRepository.AddAsync(toAccount);

        await _unitOfWork.Commit(cancellationToken);

        return new TransferResponse
        {
            FromAccountId = outAccount.Id,
            ToAcountId = inAccount.Id,
            Amount = request.Amount,
            TransactionType = TransactionType.Transfer,
            Description = fromAccount.Description,
            Timestamp = fromAccount.Timestamp
        };
    }
}