using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Account.Queries.GetBalance;

public class GetBalanceHandler : IRequestHandler<GetBalanceRequest, GetBalanceResponse>
{
    
    private readonly IAccountRepository _accountRepository;

    public GetBalanceHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<GetBalanceResponse> Handle(GetBalanceRequest request, CancellationToken cancellationToken)
    {
        var balance = _accountRepository.GetBalance(request.Id);
        
        return new GetBalanceResponse($"Balance: {balance}");
    }
}