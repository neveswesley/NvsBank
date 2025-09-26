using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Infrastructure.Exceptions;

namespace NvsBank.Application.UseCases.Account.Queries;

public interface GetBalance
{
    public sealed record GetBalanceCommand(Guid AccountId) : IRequest<decimal>;

    public sealed record GetBalanceRequest()
    {
        public GetBalanceCommand ToCommand(Guid accountId) => new GetBalanceCommand(accountId);
    }

    public class GetBalanceHandler : IRequestHandler<GetBalanceCommand, decimal>
    {
        
        private readonly IAccountRepository _accountRepository;

        public GetBalanceHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<decimal> Handle(GetBalanceCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
            if (account == null)
                throw new NotFoundException("Account not found.");
            
            return account.Balance;
            
        }
    }
    
}