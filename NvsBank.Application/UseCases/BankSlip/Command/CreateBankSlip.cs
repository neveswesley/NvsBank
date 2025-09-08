using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Application.Shared.Extras;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.BankSlip.Command;

public abstract class CreateBankSlip
{
    public sealed record CreateBankSlipCommand : IRequest<BankSlipResponse>
    {
        public decimal Amount { get; set; }
        public Guid PayeeId { get; set; }
        public Guid PayerId { get; set; }
    }
    
    public class CreateBankSlipHandler : IRequestHandler<CreateBankSlipCommand, BankSlipResponse>
    {
        private readonly IBankSlipRepository _bankSlipRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateBankSlipHandler(IBankSlipRepository bankSlipRepository, IUnitOfWork unitOfWork, IMapper mapper, IAccountRepository accountRepository)
        {
            _bankSlipRepository = bankSlipRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountRepository = accountRepository;
        }

        public async Task<BankSlipResponse> Handle(CreateBankSlipCommand request, CancellationToken cancellationToken)
        {
            var payeeAccount = await _accountRepository.GetByIdAsync(request.PayeeId);
            if (payeeAccount == null)
                throw new ApplicationException("Payee not found");
        
            var digitableLine = BankSlipGenerator.GenerateDigitableLine(request.Amount, DateTime.Today.AddDays(3), request.PayeeId);
        
            var bankSlip = new Domain.Entities.BankSlip
            {
                Id = Guid.NewGuid(),
                DigitableLine = digitableLine,
                Amount = request.Amount,
                DueDate = DateTime.Today.AddDays(3),
                PayeeId = request.PayeeId,
                PayerId = request.PayerId,
                IsPaid = false
            };
       
            await _bankSlipRepository.AddAsync(bankSlip);
        
            await _unitOfWork.Commit(cancellationToken);
        
            return _mapper.Map<BankSlipResponse>(bankSlip);
        }
    }
    
}