using MediatR;
using NvsBank.Application.Exceptions;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.PixKey.Commands;

public sealed record RemoveKeyValueCommand (Guid PixKeyId, Guid UserId) : IRequest<Unit>;

public class RemoveKeyValueHandler : IRequestHandler<RemoveKeyValueCommand, Unit>
{
    private readonly IPixKeyRepository _pixKeyRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveKeyValueHandler(IPixKeyRepository pixKeyRepository, IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _pixKeyRepository = pixKeyRepository;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RemoveKeyValueCommand request, CancellationToken cancellationToken)
    {
        var pixKey = await _pixKeyRepository.GetByIdAsync(request.PixKeyId);
        if (pixKey == null || pixKey.Status != PixKeyStatus.Active)
            throw new NotFoundException("Pix key not found");
        
        bool isOwner = await _pixKeyRepository.IsUserAccountAsync(request.UserId, pixKey.AccountId, cancellationToken);
        if (!isOwner)
            throw new UnauthorizedException("You cannot delete a Pix key that does not belong to this account");
        
        pixKey.Status = PixKeyStatus.Deleted;
        _pixKeyRepository.UpdateAsync(pixKey);
        await _unitOfWork.Commit(cancellationToken);
        
        return Unit.Value;
    }
}