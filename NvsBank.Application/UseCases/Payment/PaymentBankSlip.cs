using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Payment;

public class PaymentBankSlip
{

    public class PaymentBankSlipCommand : IRequest<Domain.Entities.Payment>
    {
        public string DigitableLine { get; set; } = string.Empty;
        public Guid PayerAccountId { get; set; }
        public string IdempotencyKey { get; set; } = string.Empty;
    }

    public class PaymentBankSlipHandler : IRequestHandler<PaymentBankSlipCommand, Domain.Entities.Payment>
    {
        private readonly IBankSlipRepository _bankSlipRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentBankSlipHandler(IBankSlipRepository bankSlipRepository, IAccountRepository accountRepository, IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
        {
            _bankSlipRepository = bankSlipRepository;
            _accountRepository = accountRepository;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Domain.Entities.Payment> Handle(PaymentBankSlipCommand request, CancellationToken cancellationToken)
        {
            var bankSlip = await _bankSlipRepository.GetByDigitableLine(request.DigitableLine);
            if (bankSlip == null)
                throw new ApplicationException("No bank slip found");
            
            var existingPayment = await _paymentRepository.GetByIdempotencyKeyAsync(bankSlip.Id, request.IdempotencyKey, cancellationToken);
            if (existingPayment != null)
                return existingPayment;
            
            if (bankSlip.IsPaid)
                throw new ApplicationException("Bank slip is paid");
            
            var payee = await _accountRepository.GetByIdAsync(bankSlip.AccuntPayeeId, cancellationToken);
            var payer = await _accountRepository.GetByIdAsync(bankSlip.CustomerPayerId, cancellationToken);


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
                PayeeAccountId = bankSlip.AccuntPayeeId,
                Amount = payableAmount,
                PaidAt = DateTime.Now
            };
            
            await _paymentRepository.AddAsync(payment, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            
            return payment;
        }
    }
}