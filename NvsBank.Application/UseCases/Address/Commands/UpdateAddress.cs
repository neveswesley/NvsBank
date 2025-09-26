using MediatR;
using Microsoft.AspNetCore.Identity;
using NvsBank.Application.Exceptions;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Address.Commands;

public abstract class UpdateAddress
{
    
    public sealed record UpdateAddressCommand(
        Guid Id,
        string Street,
        string Number,
        string City,
        string State,
        string ZipCode) : IRequest<AddressResponse>;

    public sealed record UpdateAddressRequest(
        string Street,
        string Number,
        string City,
        string State,
        string ZipCode);
    
    public class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, AddressResponse>
    {
    
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAddressRepository _addressRepository;
        private readonly UserManager<User> _userManager;
        private readonly ICustomerRepository _customerRepository;

        public UpdateAddressHandler(IUnitOfWork unitOfWork, IAddressRepository addressRepository, UserManager<User> userManager, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _addressRepository = addressRepository;
            _userManager = userManager;
            _customerRepository = customerRepository;
        }

        public async Task<AddressResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);
            if (customer == null)
                throw new NotFoundException("Customer not found");

            var address = await _addressRepository.GetByIdAsync(customer.AddressId);
        
            address.UpdateAddress(request.Street, request.Number, request.City, request.State, request.ZipCode);
        
            _addressRepository.UpdateAsync(address);
        
            await _unitOfWork.Commit(cancellationToken);
        
            return new AddressResponse(address.Id, address.Street, address.Number, address.City, address.State, address.ZipCode, address.CustomerId);
        }
    }
    
}