using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Account.Queries;

public abstract class GetAllAccount
{
    public sealed record GetAllAccountQuery : IRequest<List<AccountResponse>>;

    public class GetAllAccountHandler : IRequestHandler<GetAllAccountQuery, List<AccountResponse>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAllAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<List<AccountResponse>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _accountRepository.GetAllAsync(cancellationToken);

            return accounts.Select(x => new AccountResponse
            {
                Id = x.Id,
                AccountNumber = x.AccountNumber,
                Branch = x.Branch,
                AccountType = x.AccountType,
                Balance = x.Balance,
                OverdraftLimit = x.OverdraftLimit,
                OpeningDate = x.OpeningDate,
                ClosingDate = x.ClosingDate,
                Status = x.Status,
                CustomerId = x.CustomerId
            }).ToList();
        }
    }
}