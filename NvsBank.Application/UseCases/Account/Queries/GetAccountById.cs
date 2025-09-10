using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Account.Queries;

public abstract class GetAccountById
{
    public sealed record GetAccountByIdQuery(Guid AccountId) : IRequest<Domain.Entities.Account>;

    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, Domain.Entities.Account>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountByIdHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Domain.Entities.Account> Handle(GetAccountByIdQuery request,
            CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
            if (account == null)
                throw new ApplicationException("Account not found.");
            
            return account;
        }
    }
}