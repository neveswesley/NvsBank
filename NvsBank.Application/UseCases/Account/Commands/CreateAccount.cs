using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;
using NvsBank.Infrastructure.Exceptions;

namespace NvsBank.Application.UseCases.Account.Commands;

public sealed record CreateAccountCommand : IRequest<AccountResponse>
{
    public AccountType AccountType { get; set; }
    public Guid CustomerId { get; set; }
}

public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, AccountResponse>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAccountHandler(IAccountRepository accountRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AccountResponse> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var account = _mapper.Map<Domain.Entities.Account>(command);
        
        var customer = await _customerRepository.GetByIdWithAccountAsync(command.CustomerId);
        
        if (customer == null)
            throw new NotFoundException("Customer not found.");

        if (customer.Status != PersonStatus.Active)
            throw new UnauthorizedException("Customer is not active.");

        if (customer.Accounts.Any(x => x.AccountType == command.AccountType))
            throw new BadRequestException("Customer already has an account of this type.");
        
        customer.AccountId = account.Id;

        await _accountRepository.CreateAsync(account);
        _unitOfWork.Customers.UpdateAsync(customer);
        

        await _unitOfWork.Commit(cancellationToken);

        return _mapper.Map<AccountResponse>(account);
    }
}