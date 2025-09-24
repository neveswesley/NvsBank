using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Repositories;

namespace NvsBank.Application.UseCases.Payment;

public class PaymentBankSlip
{
    public class PaymentBankSlipCommand : IRequest<Domain.Entities.Payment>
    {
        public string DigitableLine { get; set; } = string.Empty;
        public Guid PayerAccountId { get; private set; }
        public string IdempotencyKey { get; set; } = string.Empty;
        
        public void SetPayerAccount(Guid accountId) => PayerAccountId = accountId;
    }

    public sealed record PayBankSlipRequest(string DigitableLine, Guid PayerAccountId, string IdempotencyKey) : IRequest<PaymentBankSlipCommand>;

    public class PaymentBankSlipHandler : IRequestHandler<PaymentBankSlipCommand, Domain.Entities.Payment>
    {
        private readonly IBankSlipRepository _bankSlipRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentBankSlipHandler(IBankSlipRepository bankSlipRepository, IAccountRepository accountRepository,
            IPaymentRepository paymentRepository, IUnitOfWork unitOfWork, ITransactionRepository transactionRepository)
        {
            _bankSlipRepository = bankSlipRepository;
            _accountRepository = accountRepository;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _transactionRepository = transactionRepository;
        }

        public async Task<Domain.Entities.Payment> Handle(PaymentBankSlipCommand request,
            CancellationToken cancellationToken)
        {
            var bankSlip = await _bankSlipRepository.GetByDigitableLine(request.DigitableLine);
            if (bankSlip == null)
                throw new ApplicationException("No bank slip found");

            var existingPayment =
                await _paymentRepository.GetByIdempotencyKeyAsync(bankSlip.Id, request.IdempotencyKey,
                    cancellationToken);

            if (existingPayment != null)
                return existingPayment;

            if (bankSlip.IsPaid)
                throw new ApplicationException("Bank slip is paid");

            var payee = await _accountRepository.GetByIdAsync(bankSlip.AccountPayeeId, cancellationToken);

            var payer = await _accountRepository.GetByIdAsync(request.PayerAccountId, cancellationToken);

            var payableAmount = bankSlip.CalculatePayableAmount(DateTime.Now);

            if (payer.Balance < payableAmount)
                throw new ApplicationException("Insufficient balance");

            payee.Deposit(payableAmount);
            payer.Withdraw(payableAmount);
            bankSlip.MarkAsPaid();

            var payment = new Domain.Entities.Payment
            {
                BankSlipId = bankSlip.Id,
                PayerAccountId = request.PayerAccountId,
                PayeeAccountId = bankSlip.AccountPayeeId,
                Amount = payableAmount,
                PaidAt = DateTime.Now
            };

            var transactionFrom = new Domain.Entities.Transaction
            {
                AccountId = payer.Id,
                Amount = payableAmount,
                NewBalance = payer.Balance,
                OldBalance = payer.Balance + payableAmount,
                TransactionType = TransactionType.Payment,
                Description = "Bill Payment",
                Timestamp = DateTime.Now
            };

            var transactionTo = new Domain.Entities.Transaction
            {
                AccountId = payee.Id,
                Amount = payableAmount,
                NewBalance = payee.Balance,
                OldBalance = payee.Balance - payableAmount,
                TransactionType = TransactionType.Payment,
                Description = "Bill Receipt",
                Timestamp = DateTime.Now
            };

            await _transactionRepository.AddAsync(transactionFrom);
            await _transactionRepository.AddAsync(transactionTo);
            await _paymentRepository.AddAsync(payment, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return payment;
        }
    }
}