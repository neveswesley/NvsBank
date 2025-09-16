using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.PixKey.Commands;

public sealed record RemoveKeyValueCommand (PixKeyType KeyType) : IRequest<RemoveKeyValueResponse>;

public sealed record RemoveKeyValueResponse(string Message);

public class RemoveKeyValueHandler : IRequestHandler<RemoveKeyValueCommand, RemoveKeyValueResponse>
{
    
    private readonly IPixKeyRepository _pixKeyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveKeyValueHandler(IPixKeyRepository pixKeyRepository, IUnitOfWork unitOfWork)
    {
        _pixKeyRepository = pixKeyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RemoveKeyValueResponse> Handle(RemoveKeyValueCommand request, CancellationToken cancellationToken)
    {
        _pixKeyRepository.RemoveKeyValue(request.KeyType);
        await _unitOfWork.Commit(cancellationToken);
        return new RemoveKeyValueResponse("Key value removed");
    }
}