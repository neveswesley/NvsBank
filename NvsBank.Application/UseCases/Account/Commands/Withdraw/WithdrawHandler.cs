using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Account.Commands.Withdraw;

public class WithdrawHandler : IRequestHandler<WithdrawRequest, WithdrawResponse>
{
    
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WithdrawHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WithdrawResponse> Handle(WithdrawRequest request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.Id);
        _accountRepository.Withdraw(request.Id, request.Amount);
        
        await _unitOfWork.Commit(cancellationToken);

        return new WithdrawResponse($"New balance: {account.Balance}");
    }
}