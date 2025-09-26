using MediatR;
using NvsBank.Application.Exceptions;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Account.Commands;

public abstract class UpdateAccountStatus
{
    public class UpdateAccountStatusCommand : IRequest<ChangeAccountStatusCommand>
    {
        public AccountStatus Status { get; set; }
        public string? Reason { get; set; }
    }

    public sealed record ChangeAccountStatusCommand(Guid AccountId, AccountStatus Status, string? Reason)
        : IRequest<AccountStatusResponse>;

    public class UpdateAccountStatusHandler : IRequestHandler<ChangeAccountStatusCommand, AccountStatusResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAccountStatusHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountStatusResponse> Handle(ChangeAccountStatusCommand request,
            CancellationToken cancellationToken)
        {
            var account = _accountRepository.GetByIdAsync(request.AccountId, cancellationToken).Result;
            if (account == null)
                throw new NotFoundException("Account not found.");

            var oldStatus = account.AccountStatus;

            if (oldStatus == AccountStatus.Closed && request.Status == AccountStatus.Active)
                throw new BadRequestException("Closed account cannot be reactivated.");

            account.AccountStatus = request.Status;
            account.StatusReason = request.Reason;
            account.ModifiedDate = DateTime.Now;


            _accountRepository.UpdateAsync(account);
            await _unitOfWork.Commit(cancellationToken);

            return new AccountStatusResponse
            {
                AccountId = account.Id,
                OldStatus = oldStatus,
                NewStatus = account.AccountStatus,
                Reason = account.StatusReason,
                ChangeAt = account.ModifiedDate
            };
        }
    }
}