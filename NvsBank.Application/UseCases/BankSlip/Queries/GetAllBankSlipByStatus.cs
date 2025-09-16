using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.BankSlip.Queries;

public class GetAllBankSlipByStatus
{
    public sealed record GetAllBankSlipByStatusQuery (bool Status, int Page, int PageSize) : IRequest<PagedResult<BankSlipResponse>>;

    public class GetAllBankSlipByStatusHandler : IRequestHandler<GetAllBankSlipByStatusQuery, PagedResult<BankSlipResponse>>
    {
        private readonly IBankSlipRepository _repository;

        public GetAllBankSlipByStatusHandler(IBankSlipRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<BankSlipResponse>> Handle(GetAllBankSlipByStatusQuery request,
            CancellationToken cancellationToken)
        {
            var bankSlips = await _repository.GetAllBankSlipByStatus(request.Status, request.Page, request.PageSize);

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