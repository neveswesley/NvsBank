using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Infrastructure.Repositories;

namespace NvsBank.Application.UseCases.Transaction.Commands;

public abstract class Transfer
{
    public sealed record TransferCommand(Guid To, Guid From, decimal Amount, string Description)
        : IRequest<TransferResponse>;

    public class TransferHandler : IRequestHandler<TransferCommand, TransferResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferHandler(IAccountRepository accountRepository, ITransactionRepository transactionRepository,
            ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransferResponse> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            var inAccount = await _accountRepository.GetByIdAsync(request.To, cancellationToken);
            if (inAccount == null)
                throw new ApplicationException("Destination account not found.");
            if (inAccount.AccountStatus != AccountStatus.Active)
                throw new ApplicationException("Account is not active.");

            var inCustomer = await _customerRepository.GetByIdAsync(inAccount.CustomerId);
            if (inCustomer.CustomerStatus != CustomerStatus.Active)
                throw new ApplicationException("Customer is not active.");

            var outAccount = await _accountRepository.GetByIdAsync(request.From, cancellationToken);
            if (outAccount == null)
                throw new ApplicationException("Source account not found.");
            if (outAccount.AccountStatus != AccountStatus.Active)
                throw new ApplicationException("Account is not active.");
            
            var outCustomer = await _customerRepository.GetByIdAsync(outAccount.CustomerId);
            if (outCustomer.CustomerStatus != CustomerStatus.Active)
                throw new ApplicationException("Customer is not active.");

            if (outAccount.Balance < request.Amount)
                throw new ApplicationException("Insufficient balance.");

            if (outAccount.Id == inAccount.Id)
                throw new ApplicationException("Source and destination account cannot be the same.");

            outAccount.Balance -= request.Amount;
            inAccount.Balance += request.Amount;

            _accountRepository.UpdateAsync(inAccount);
            _accountRepository.UpdateAsync(outAccount);

            var fromAccount = new Domain.Entities.Transaction
            {
                AccountId = outAccount.Id,
                NewBalance = outAccount.Balance,
                OldBalance = outAccount.Balance + request.Amount,
                Amount = request.Amount,
                TransactionType = TransactionType.Transfer,
                Description = request.Description
            };

            await _transactionRepository.AddAsync(fromAccount);

            var toAccount = new Domain.Entities.Transaction
            {
                AccountId = inAccount.Id,
                NewBalance = inAccount.Balance,
                OldBalance = inAccount.Balance - request.Amount,
                Amount = request.Amount,
                TransactionType = TransactionType.Transfer,
                Description = request.Description
            };

            await _transactionRepository.AddAsync(toAccount);

            await _unitOfWork.Commit(cancellationToken);

            return new TransferResponse
            {
                FromAccountId = outAccount.Id,
                ToAcountId = inAccount.Id,
                Amount = request.Amount,
                TransactionType = TransactionType.Transfer,
                Description = fromAccount.Description,
                Timestamp = fromAccount.Timestamp
            };
        }
    }
}