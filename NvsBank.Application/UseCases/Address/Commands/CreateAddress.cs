using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Address.Commands;

public abstract class CreateAddress
{
    public sealed record CreateAddressCommand : IRequest<AddressResponse>
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Guid CustomerId { get; set; }
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
            if (address.CustomerId == null)
                throw new ApplicationException("Customer not found.");

            var customer = await _unitOfWork.Customers.GetByIdAsync(address.CustomerId);
            if (customer == null)
                throw new ApplicationException("Customer not found.");

            customer.AddressId = address.Id;
        
            _unitOfWork.Customers.UpdateAsync(customer);

            await _unitOfWork.Commit(cancellationToken);
        
            var response = _mapper.Map<AddressResponse>(address);
            
            response.CustomerId = customer.Id;

            return response;
        }
    }
    
}