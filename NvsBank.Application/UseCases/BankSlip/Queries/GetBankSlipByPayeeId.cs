using AutoMapper;
using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.BankSlip.Queries;

public class GetBankSlipByPayeeId
{
    public sealed record GetBankSlipByPayeeIdQuery(Guid PayeeId) : IRequest<BankSlipResponse>;

    public class GetBankSlipByPayeeIdHandler : IRequestHandler<GetBankSlipByPayeeIdQuery, BankSlipResponse>
    {
        private readonly IBankSlipRepository _bankSlipRepository;
        private readonly IMapper _mapper;

        public GetBankSlipByPayeeIdHandler(IBankSlipRepository bankSlipRepository, IMapper mapper)
        {
            _bankSlipRepository = bankSlipRepository;
            _mapper = mapper;
        }

        public async Task<BankSlipResponse> Handle(GetBankSlipByPayeeIdQuery request,
            CancellationToken cancellationToken)
        {
            var response = await _bankSlipRepository.GetByPayeeId(request.PayeeId);
            return _mapper.Map<BankSlipResponse>(response);
        }
    }
}