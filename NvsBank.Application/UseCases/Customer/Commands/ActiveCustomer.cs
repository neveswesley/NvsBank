using MediatR;
using NvsBank.Application.Exceptions;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Commands;

public class ActiveCustomer
{
    public sealed record ActiveCustomerCommand(Guid CustomerId) : IRequest<AccountResponse>;

    public sealed record ActiveCustomerRequest : IRequest<ActiveCustomerCommand>;

    public class ActiveCustomerHandler : IRequestHandler<ActiveCustomerCommand, AccountResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;

        public ActiveCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork,
            IAccountRepository accountRepository)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
        }

        public async Task<AccountResponse> Handle(ActiveCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

            if (customer.Status == PersonStatus.Active)
                throw new BadRequestException("The customer account is already active.");

            customer.Status = PersonStatus.Active;

            var account = new Domain.Entities.Account
            {
                CustomerId = request.CustomerId,
                AccountType = AccountType.Checking
            };
            
            await _accountRepository.CreateAsync(account);

            customer.AccountId = account.Id;
            
            await _unitOfWork.Commit(cancellationToken);

            return new AccountResponse
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                Branch = account.Branch,
                AccountType = account.AccountType,
                Balance = account.Balance,
                OverdraftLimit = account.OverdraftLimit,
                OpeningDate = DateTime.Now,
                ClosingDate = null,
                Status = account.AccountStatus,
                CustomerId = account.CustomerId
            };
        }
    }
}