using MediatR;
using NvsBank.Application.Exceptions;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.BankSlip.Queries;

public class GetBankSlipByDigitableLine
{
    public sealed record GetBankSlipByDigitableLineQuery (string DigitableLine): IRequest<BankSlipResponse>;
    
    public class GetBankSlipByDigitableLineHandler : IRequestHandler<GetBankSlipByDigitableLineQuery, BankSlipResponse>
    {
        
        private readonly IBankSlipRepository _repository;

        public GetBankSlipByDigitableLineHandler(IBankSlipRepository repository)
        {
            _repository = repository;
        }

        public async Task<BankSlipResponse> Handle(GetBankSlipByDigitableLineQuery request, CancellationToken cancellationToken)
        {
            var bankSlip = await _repository.GetByDigitableLine(request.DigitableLine);
            if (bankSlip == null)
                throw new NotFoundException("No bank slip found");
            
            return new BankSlipResponse
            {
                Id = bankSlip.Id,
                DigitableLine = bankSlip.DigitableLine,
                Amount = bankSlip.Amount,
                DueDate = bankSlip.DueDate,
                AccountPayeeId = bankSlip.AccountPayeeId,
                CustomerPayerId = bankSlip.CustomerPayerId,
                IsPaid = bankSlip.IsPaid,
                PaidAt = bankSlip.PaidAt
            };
        }
    }
}