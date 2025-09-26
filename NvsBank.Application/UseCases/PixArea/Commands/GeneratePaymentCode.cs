using MediatR;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Payment;

public class GeneratePaymentCode
{
    public sealed record GenerateReceiptCommand(decimal Amount, Guid AccountId) : IRequest<string>;
    
    public sealed record GenerateReceiptRequest(decimal Amount)
    {
        public GenerateReceiptCommand ToCommand(Guid accountId)
            => new GenerateReceiptCommand(Amount, accountId);
    }

    public class GenerateReceiptHandler : IRequestHandler<GenerateReceiptCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentCodeRepository _paymentCodeRepository;

        public GenerateReceiptHandler(IUnitOfWork unitOfWork, IPaymentCodeRepository paymentCodeRepository)
        {
            _unitOfWork = unitOfWork;
            _paymentCodeRepository = paymentCodeRepository;
        }

        public async Task<string> Handle(GenerateReceiptCommand request, CancellationToken cancellationToken)
        {
            var receipt = new PaymentCode
            {
                AccountId = request.AccountId,
                RandomPaymentCode = PixCodeGenerator.GenerateNumericCodeWithLuhn(32),
                Amount = request.Amount,
            };

            await _paymentCodeRepository.CreateAsync(receipt);
            await _unitOfWork.Commit(cancellationToken);
            return receipt.RandomPaymentCode;
        }
    }
}