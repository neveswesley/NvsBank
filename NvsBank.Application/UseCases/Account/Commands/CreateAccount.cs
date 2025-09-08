using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Account.Commands;

public sealed record CreateAccountRequest : IRequest<AccountResponse>
{
    public AccountType AccountType { get; set; }
    public Guid CustomerId { get; set; }
}

public class CreateAccountHandler : IRequestHandler<CreateAccountRequest, AccountResponse>
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

    public async Task<AccountResponse> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var account = _mapper.Map<Domain.Entities.Account>(request);

        await _accountRepository.CreateAsync(account);

        var customer = await _unitOfWork.Customers.GetByIdAsync(account.CustomerId);
        if (customer == null)
            throw new ApplicationException("Customer not found.");

        customer.AccountId = account.Id;

        _unitOfWork.Customers.UpdateAsync(customer);

        await _unitOfWork.Commit(cancellationToken);

        return _mapper.Map<AccountResponse>(account);
    }
}