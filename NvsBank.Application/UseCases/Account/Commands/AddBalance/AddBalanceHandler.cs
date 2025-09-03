using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Account.Commands.AddBalance;

public class AddBalanceHandler : IRequestHandler<AddBalanceRequest, AddBalanceResponse>
{
    
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddBalanceHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AddBalanceResponse> Handle(AddBalanceRequest request, CancellationToken cancellationToken)
    {
        _accountRepository.AddBalance(request.Id, request.Amount);
        var account = await _accountRepository.GetByIdAsync(request.Id);

        await _unitOfWork.Commit(cancellationToken);

        return new AddBalanceResponse($"New balance: {account.Balance}");
    }
}