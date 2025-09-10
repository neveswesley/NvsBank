using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Account.Queries;

public class GetActiveAccounts
{
    public sealed record GetActiveAccountsQuery : IRequest<List<AccountResponse>>;

    public class GetActiveAccountHandler : IRequestHandler<GetActiveAccountsQuery, List<AccountResponse>>
    {
        
        private readonly IAccountRepository _accountRepository;

        public GetActiveAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<List<AccountResponse>> Handle(GetActiveAccountsQuery request, CancellationToken cancellationToken)
        {
            var activesAccount = await _accountRepository.GetActiveAsync(cancellationToken);
            
            return activesAccount.Select(x=> new AccountResponse
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
        }
    }
}