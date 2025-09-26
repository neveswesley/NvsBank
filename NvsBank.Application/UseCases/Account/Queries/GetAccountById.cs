using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Application.Exceptions;

namespace NvsBank.Application.UseCases.Account.Queries;

public abstract class GetAccountById
{
    public sealed record GetAccountByIdQuery(Guid AccountId) : IRequest<AccountResponse>;

    public sealed record GetAccountByIdRequest(Guid AccountId) : IRequest<AccountResponse>;

    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountResponse>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountByIdHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountResponse> Handle(GetAccountByIdQuery request,
            CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
            
            if (account == null)
                throw new NotFoundException("Account not found.");

            return new AccountResponse
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                Branch = account.Branch,
                AccountType = account.AccountType,
                Balance = account.Balance,
                OverdraftLimit = account.OverdraftLimit,
                OpeningDate = account.OpeningDate,
                ClosingDate = account.ClosingDate,
                Status = account.AccountStatus,
                CustomerId = account.CustomerId
            };
        }
    }
}