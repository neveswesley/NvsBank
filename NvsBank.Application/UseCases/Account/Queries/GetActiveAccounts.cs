using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Account.Queries;

public class GetActiveAccounts
{
    public sealed record GetActiveAccountsQuery(int Page, int PageSize) : IRequest<PagedResult<AccountResponse>>;

    public class GetActiveAccountHandler : IRequestHandler<GetActiveAccountsQuery, PagedResult<AccountResponse>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetActiveAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<PagedResult<AccountResponse>> Handle(GetActiveAccountsQuery request,
            CancellationToken cancellationToken)
        {
            var activesAccountPaged = await _accountRepository.GetActiveAsync(request.Page, request.PageSize);

            var accountResponses = activesAccountPaged.Items.Select(x => new AccountResponse
            {
                Id = x.Id,
                AccountNumber = x.AccountNumber,
                Branch = x.Branch,
                AccountType = x.AccountType,
                Balance = x.Balance,
                OverdraftLimit = x.OverdraftLimit,
                OpeningDate = x.OpeningDate,
                ClosingDate = x.ClosingDate,
                Status = x.AccountStatus,
                CustomerId = x.CustomerId
            }).ToList();
            
            return new PagedResult<AccountResponse>
            {
                Items = accountResponses,
                Page = activesAccountPaged.Page,
                PageSize = activesAccountPaged.PageSize,
                TotalCount = activesAccountPaged.TotalCount
            }; 
        }
    }
}