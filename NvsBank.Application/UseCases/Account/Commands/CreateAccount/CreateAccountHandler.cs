using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Application.UseCases.Account.Commands.CreateAccount;

namespace NvsBank.Application.UseCases.Account.CreateAccount;

public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, CreateAccountResponse>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAccountHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateAccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = _mapper.Map<Domain.Entities.Account>(request);
        
        await _accountRepository.CreateAsync(account);
        
        var customer = await _unitOfWork.Customers.GetByIdAsync(account.CustomerId);
        if (customer == null)
            throw new ApplicationException("Custoemr not found.");
        
        customer.AccountId = account.Id;
        
        _unitOfWork.Customers.UpdateAsync(customer);

        await _unitOfWork.Commit(cancellationToken);
        
        return _mapper.Map<CreateAccountResponse>(account);
    }
}