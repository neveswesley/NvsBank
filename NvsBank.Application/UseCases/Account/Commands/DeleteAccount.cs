using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Infrastructure.Exceptions;

namespace NvsBank.Application.UseCases.Account.Commands;

public class DeleteAccount
{
    public sealed record Query (Guid Id) : IRequest<DeleteAccountResponse>;

    public sealed record DeleteAccountResponse(string Message);
    
    public class Handler : IRequestHandler<Query, DeleteAccountResponse>
    {
        
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteAccountResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.Id, cancellationToken);
            if (account == null)
                throw new NotFoundException("Account not found");
            
            _accountRepository.InactiveAsync(account);
            await _unitOfWork.Commit(cancellationToken);
            return new DeleteAccountResponse("Account has been closed.");
        }
    }
}