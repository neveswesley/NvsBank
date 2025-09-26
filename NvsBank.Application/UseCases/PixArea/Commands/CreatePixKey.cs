using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.PixKey.Commands;

public class CreatePixKey
{
    public sealed record CreatePixKeyCommand(Guid AccountId, string KeyValue, PixKeyType KeyType)
        : IRequest<PixKeyResponse>;
    
    public sealed record CreatePixKeyRequest(string Value, PixKeyType Type)
    {
        public CreatePixKeyCommand ToCommand(Guid accountId)
            => new CreatePixKeyCommand(accountId, Value, Type);
    }}

public class CreatePixKeyHandler : IRequestHandler<CreatePixKey.CreatePixKeyCommand, PixKeyResponse>
{
    private readonly IPixKeyRepository _pixKeyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePixKeyHandler(IPixKeyRepository pixKeyRepository, IUnitOfWork unitOfWork)
    {
        _pixKeyRepository = pixKeyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PixKeyResponse> Handle(CreatePixKey.CreatePixKeyCommand request,
        CancellationToken cancellationToken)
    {
        var pixkeysExists = await _pixKeyRepository.GetAllAsync();


        if (pixkeysExists.Any(x => x.AccountId == request.AccountId && request.KeyType == PixKeyType.CPF && x.Status == PixKeyStatus.Active))
        {
            throw new Exception($"A CPF Pix key is already registered for this account.");
        }

        if (pixkeysExists.Any(x => x.KeyValue == request.KeyValue && x.Status == PixKeyStatus.Active))
        {
            throw new ApplicationException(
                "The Pix key you are trying to register is already associated with another account");
        }


        var pixKey = new Domain.Entities.PixArea
        {
            AccountId = request.AccountId,
            KeyType = request.KeyType,
            KeyValue = request.KeyValue
        };

        if (pixKey.KeyType == PixKeyType.EVP)
        {
            var newPixKey = await _pixKeyRepository.GenerateUniqueEvPAsync(cancellationToken: cancellationToken);
            pixKey.KeyValue = newPixKey;
        }

        await _pixKeyRepository.CreateAsync(pixKey);
        await _unitOfWork.Commit(cancellationToken);

        return new PixKeyResponse
        {
            AccountId = request.AccountId,
            KeyType = pixKey.KeyType,
            KeyValue = pixKey.KeyValue,
            Status = pixKey.Status
        };
    }
}