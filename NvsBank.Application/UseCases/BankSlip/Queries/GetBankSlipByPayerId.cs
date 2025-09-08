using AutoMapper;
using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.BankSlip.Queries;

public class GetBankSlipByPayerId
{
    public sealed record GetBankSlipByPayerIdQuery (Guid PayerId) : IRequest<BankSlipResponse>;
    
    public class GetBankSlipByPayerHandler : IRequestHandler<GetBankSlipByPayerIdQuery, BankSlipResponse>
    {
    
        private readonly IMapper _mapper;
        private readonly IBankSlipRepository _bankSlipRepository;

        public GetBankSlipByPayerHandler(IMapper mapper, IBankSlipRepository bankSlipRepository)
        {
            _mapper = mapper;
            _bankSlipRepository = bankSlipRepository;
        }

        public async Task<BankSlipResponse> Handle(GetBankSlipByPayerIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _bankSlipRepository.GetByPayerId(request.PayerId);
            return _mapper.Map<BankSlipResponse>(response);
        }
    }
    
}