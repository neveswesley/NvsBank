using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Application.UseCases.Transaction.Commands;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Repositories;

namespace NvsBank.Application.UseCases.PixKey.Commands;

public sealed record PixTransferCommand(Guid FromAccountId, string ToPixKey, decimal Amount, string Description)
    : IRequest<PixTransferResponse>;

public class PixTransferHandler : IRequestHandler<PixTransferCommand, PixTransferResponse>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPixKeyRepository _pixKeyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PixTransferHandler(IAccountRepository accountRepository, IPaymentRepository paymentRepository,
        ITransactionRepository transactionRepository, IPixKeyRepository pixKeyRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _paymentRepository = paymentRepository;
        _transactionRepository = transactionRepository;
        _pixKeyRepository = pixKeyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PixTransferResponse> Handle(PixTransferCommand request, CancellationToken cancellationToken)
    {
        var fromAccountId = await _accountRepository.GetByIdAsync(request.FromAccountId, cancellationToken);
        if (fromAccountId == null)
            throw new ApplicationException("Account not found");

        var toPixKey = await _pixKeyRepository.GetPixKeyByIdAsync(request.ToPixKey);
        if (toPixKey == null)
            throw new ApplicationException("PixKey not found");

        var transactionSource = new Domain.Entities.Transaction
        {
            AccountId = fromAccountId.Id,
            Amount = request.Amount,
            NewBalance = fromAccountId.Balance - request.Amount,
            OldBalance = fromAccountId.Balance,
            TransactionType = TransactionType.Pix,
            Description = request.Description,
            Timestamp = DateTime.Now
        };

        var transactionDestination = new Domain.Entities.Transaction
        {
            AccountId = toPixKey.AccountId,
            Amount = request.Amount,
            NewBalance = toPixKey.Account.Balance + request.Amount,
            OldBalance = toPixKey.Account.Balance,
            TransactionType = TransactionType.Pix,
            Description = request.Description,
            Timestamp = DateTime.Now
        };

        var account = toPixKey.AccountId;

        var toAccount = await _accountRepository.GetByIdAsync(account, cancellationToken);
        
        if (fromAccountId.Balance < request.Amount)
            throw new ApplicationException("Insufficient funds");
        
        toAccount.Deposit(request.Amount);
        fromAccountId.Withdraw(request.Amount);

        await _transactionRepository.AddAsync(transactionSource);
        await _transactionRepository.AddAsync(transactionDestination);

        await _unitOfWork.Commit(cancellationToken);

        return new PixTransferResponse
        {
            FromAccountId = fromAccountId.Id,
            ToPixKey = request.ToPixKey,
            Amount = request.Amount,
            TransactionType = TransactionType.Pix,
            Description = request.Description,
            Timestamp = transactionDestination.Timestamp,
        };
    }
}