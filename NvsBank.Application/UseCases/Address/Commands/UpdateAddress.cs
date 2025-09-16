using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Address.Commands;

public abstract class UpdateAddress
{
    public class UpdateAddressRequest : IRequest<UpdateAddressCommand>
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
    public sealed record UpdateAddressCommand(
        Guid Id,
        string Street,
        string Number,
        string City,
        string State,
        string ZipCode) : IRequest<AddressResponse>;
    
    public class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, AddressResponse>
    {
    
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAddressRepository _addressRepository;

        public UpdateAddressHandler(IUnitOfWork unitOfWork, IAddressRepository addressRepository)
        {
            _unitOfWork = unitOfWork;
            _addressRepository = addressRepository;
        }

        public async Task<AddressResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _addressRepository.GetByIdAsync(request.Id);
        
            if (address == null)
                throw new KeyNotFoundException($"User {request.Id} not found");
        
            address.UpdateAddress(request.Street, request.Number, request.City, request.State, request.ZipCode);
        
            _addressRepository.UpdateAsync(address);
        
            await _unitOfWork.Commit(cancellationToken);
        
            return new AddressResponse(address.Id, address.Street, address.Number, address.City, address.State, address.ZipCode, address.CustomerId);
        }
    }
    
}