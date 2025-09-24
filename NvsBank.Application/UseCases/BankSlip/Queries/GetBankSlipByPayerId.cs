using AutoMapper;
using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.BankSlip.Queries;

public class GetBankSlipByPayerId
{
    public sealed record GetBankSlipByPayerIdQuery (Guid PayerId, int Page, int PageSize) : IRequest<PagedResult<BankSlipResponse>>;
    
    public class GetBankSlipByPayerHandler : IRequestHandler<GetBankSlipByPayerIdQuery, PagedResult<BankSlipResponse>>
    {
    
        private readonly IMapper _mapper;
        private readonly IBankSlipRepository _bankSlipRepository;

        public GetBankSlipByPayerHandler(IMapper mapper, IBankSlipRepository bankSlipRepository)
        {
            _mapper = mapper;
            _bankSlipRepository = bankSlipRepository;
        }

        public async Task<PagedResult<BankSlipResponse>> Handle(GetBankSlipByPayerIdQuery request, CancellationToken cancellationToken)
        {
            var bankSlips = await _bankSlipRepository.GetByPayerId(request.PayerId, request.Page, request.PageSize);

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