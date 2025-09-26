using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Infrastructure.Exceptions;

namespace NvsBank.Application.UseCases.Address.Commands;

public abstract class CreateAddress
{
    public sealed record CreateAddressCommand(string Street, string Number, string City, string State, string ZipCode, Guid CustomerId)
        : IRequest<AddressResponse>;

    public sealed record CreateAddressRequest(string Street, string Number, string City, string State, string ZipCode)
    {
        public CreateAddressCommand ToCommand(Guid customerId) => new CreateAddressCommand(Street, Number, City, State, ZipCode, customerId);
    }
    
    public class CreateAddressHandler : IRequestHandler<CreateAddressCommand, AddressResponse>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAddressHandler(IAddressRepository addressRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddressResponse> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var address = _mapper.Map<Domain.Entities.Address>(request);

            await _addressRepository.CreateAsync(address);

            var customer = await _unitOfWork.Customers.GetByIdAsync(address.CustomerId);
            if (customer == null)
                throw new NotFoundException("Customer not found.");
            
            if (customer.AddressId != null)
                throw new BadRequestException("Customer address already exists.");

            customer.AddressId = address.Id;
            
            _unitOfWork.Customers.UpdateAsync(customer);

            await _unitOfWork.Commit(cancellationToken);
        
            var response = _mapper.Map<AddressResponse>(address);
            
            response.CustomerId = customer.Id;

            return response;
        }
    }
    
}