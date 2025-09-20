using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Application.Shared.Extras;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Extras;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.BankSlip.Command;

public abstract class CreateBankSlip
{
    public sealed record CreateBankSlipCommand : IRequest<BankSlipResponse>
    {
        public decimal Amount { get; set; }
        public Guid AccountPayeeId { get; set; }
        public Guid CustomerPayerId { get; set; }
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
            var accountPayee = await _accountRepository.GetByIdAsync(request.AccountPayeeId, cancellationToken);
            if (accountPayee == null)
                throw new ApplicationException("Payee not found");
            
            var customerPayer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerPayerId);
            if (customerPayer == null)
                throw new ApplicationException("Payer not found");
        
            var digitableLine = BankSlipGenerator.GenerateDigitableLine(request.Amount, DateTime.Today.AddDays(3), request.AccountPayeeId);

            var accountPayer = customerPayer.AccountId;
            
            if (request.AccountPayeeId == accountPayer)
                throw new ApplicationException("The payer account cannot be the same as the payee account.");
            
            var bankSlip = new Domain.Entities.BankSlip
            {
                Id = Guid.NewGuid(),
                DigitableLine = digitableLine,
                Amount = request.Amount,
                DueDate = DateTime.Today.AddDays(3),
                AccountPayeeId = request.AccountPayeeId,
                CustomerPayerId = request.CustomerPayerId,
                IsPaid = false
            };
       
            await _bankSlipRepository.AddAsync(bankSlip);
        
            await _unitOfWork.Commit(cancellationToken);
        
            return _mapper.Map<BankSlipResponse>(bankSlip);
        }
    }
    
}