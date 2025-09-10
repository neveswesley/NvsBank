using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Account.Queries;

public abstract class GetAllAccount
{
    public sealed record GetAllAccountQuery : IRequest<PagedResult<AccountResponse>>;

    public class GetAllAccountHandler : IRequestHandler<GetAllAccountQuery, PagedResult<AccountResponse>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAllAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<PagedResult<AccountResponse>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
        {
            var accountsPaged = await _accountRepository.GetPagedAsync();

            var accountsResponse = accountsPaged.Items.Select(x => new AccountResponse
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
                Items = accountsResponse,
                Page = accountsPaged.Page,
                PageSize = accountsPaged.PageSize,
                TotalCount = accountsPaged.TotalCount
            }; 
        }
    }
}