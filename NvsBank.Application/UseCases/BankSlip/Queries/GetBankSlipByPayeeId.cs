using AutoMapper;
using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.BankSlip.Queries;

public class GetBankSlipByPayeeId
{
    public sealed record GetBankSlipByPayeeIdQuery(Guid PayeeId, int Page, int PageSize) : IRequest<PagedResult<BankSlipResponse>>;

    public class GetBankSlipByPayeeIdHandler : IRequestHandler<GetBankSlipByPayeeIdQuery, PagedResult<BankSlipResponse>>
    {
        private readonly IBankSlipRepository _bankSlipRepository;
        private readonly IMapper _mapper;

        public GetBankSlipByPayeeIdHandler(IBankSlipRepository bankSlipRepository, IMapper mapper)
        {
            _bankSlipRepository = bankSlipRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<BankSlipResponse>> Handle(GetBankSlipByPayeeIdQuery request,
            CancellationToken cancellationToken)
        {
            var bankSlips = await _bankSlipRepository.GetByPayeeId(request.PayeeId, request.Page, request.PageSize);
            
            var response = bankSlips.Items.Select(x => new BankSlipResponse
            {
                Id = x.Id,
                DigitableLine = x.DigitableLine,
                Amount = x.Amount,
                DueDate = x.DueDate,
                AccountPayeeId = x.AccountPayeeId,
                CustomerPayerId = x.CustomerPayerId,
                IsPaid = x.IsPaid,
                PaidAt = x.PaidAt
            }).ToList();

            return new PagedResult<BankSlipResponse>
            {
                Items = response,
                Page = bankSlips.Page,
                PageSize = bankSlips.PageSize,
                TotalCount = bankSlips.TotalCount
            }; 
        }
    }
}