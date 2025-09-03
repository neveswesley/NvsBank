using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Address.Commands.UpdateAddress;

public class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, UpdateAddressResponse>
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAddressRepository _addressRepository;

    public UpdateAddressHandler(IUnitOfWork unitOfWork, IAddressRepository addressRepository)
    {
        _unitOfWork = unitOfWork;
        _addressRepository = addressRepository;
    }

    public async Task<UpdateAddressResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetByIdAsync(request.Id);
        
        if (address == null)
            throw new KeyNotFoundException($"User {request.Id} not found");
        
        address.UpdateAddress(request.Street, request.Number, request.City, request.State, request.ZipCode);
        
        _addressRepository.UpdateAsync(address);
        
        await _unitOfWork.Commit(cancellationToken);
        
        return new UpdateAddressResponse(address.Id, address.Street, address.Number, address.City, address.State, address.ZipCode, address.CustomerId);
    }
}