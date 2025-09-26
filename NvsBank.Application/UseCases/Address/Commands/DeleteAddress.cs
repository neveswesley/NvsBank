using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Infrastructure.Exceptions;

namespace NvsBank.Application.UseCases.Address.Commands;

public abstract class DeleteAddress
{
    public sealed record DeleteAddressCommand (Guid Id) : IRequest<DeleteAddressResponse>;
    
    public sealed record DeleteAddressResponse (string Message);
    
    public class DeleteAddressHandler : IRequestHandler<DeleteAddressCommand, DeleteAddressResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAddressRepository _addressRepository;

        public DeleteAddressHandler(IUnitOfWork unitOfWork, IAddressRepository addressRepository)
        {
            _unitOfWork = unitOfWork;
            _addressRepository = addressRepository;
        }

        public async Task<DeleteAddressResponse> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {

            var customer = await _unitOfWork.Customers.GetByIdAsync(request.Id);
            if (customer == null)
                throw new NotFoundException("Customer not found");
            
            var address = await _addressRepository.GetByIdAsync(customer.AddressId);
            if (address == null)
                throw new NotFoundException("Address not found");

            customer.AddressId = null;
            customer.Address = null;

            _addressRepository.DeleteAsync(address);
            await _unitOfWork.Commit(cancellationToken);

            return new DeleteAddressResponse("Address deleted successfully.");
        }
    }
}