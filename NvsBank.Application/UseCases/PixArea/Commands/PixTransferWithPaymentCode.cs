using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Repositories;

namespace NvsBank.Application.UseCases.Payment;

public class PixTransferWithPaymentCode
{
    public sealed record PaymentCodeCommand(Guid SourceAccountId, string PaymentCode) : IRequest<TransferResponse>;

    public class PaymentCodeHandler : IRequestHandler<PaymentCodeCommand, TransferResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPaymentCodeRepository _paymentCodeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentCodeHandler(IAccountRepository accountRepository, IPaymentCodeRepository paymentCodeRepository, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _paymentCodeRepository = paymentCodeRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransferResponse> Handle(PaymentCodeCommand request, CancellationToken cancellationToken)
        {
            var sourceAccount = await _accountRepository.GetByIdAsync(request.SourceAccountId, cancellationToken);
            if (sourceAccount == null)
                throw new ApplicationException("Account not found");

            var paymentCode = await _paymentCodeRepository.GetByPaymentCode(request.PaymentCode);
            if (paymentCode == null)
                throw new ApplicationException("Receipt not found");

            var receiverAccount = await _accountRepository.GetByIdAsync(paymentCode.AccountId, cancellationToken);
            if (sourceAccount == null)
                throw new ApplicationException("Account not found");

            var sourcePayment = new Domain.Entities.Transaction
            {
                AccountId = request.SourceAccountId,
                Amount = paymentCode.Amount,
                NewBalance = sourceAccount.Balance - paymentCode.Amount,
                OldBalance = sourceAccount.Balance,
                TransactionType = TransactionType.Pix,
                Description = "Payment sent via Pix",
                Timestamp = DateTime.Now
            };

            var receiverPayment = new Domain.Entities.Transaction
            {
                AccountId = paymentCode.AccountId,
                Amount = paymentCode.Amount,
                NewBalance = receiverAccount.Balance + paymentCode.Amount,
                OldBalance = receiverAccount.Balance,
                TransactionType = TransactionType.Pix,
                Description = "Payment received via Pix",
                Timestamp = DateTime.Now
            };
            
            if (paymentCode.DueDate < DateTime.Now)
                throw new ApplicationException("The Pix payment code you are trying to use has expired.");

            await _transactionRepository.AddAsync(sourcePayment);
            await _transactionRepository.AddAsync(receiverPayment);

            sourceAccount.Balance -= paymentCode.Amount;
            receiverAccount.Balance += paymentCode.Amount;
            paymentCode.IsPaid = true;

            await _unitOfWork.Commit(cancellationToken);

            return new TransferResponse
            {
                FromAccountId = sourceAccount.Id,
                ToAcountId = receiverAccount.Id,
                Amount = paymentCode.Amount,
                TransactionType = TransactionType.Pix,
                Description = "Payment mad via Pix",
                Timestamp = DateTime.Now
            };
        }
    }
}