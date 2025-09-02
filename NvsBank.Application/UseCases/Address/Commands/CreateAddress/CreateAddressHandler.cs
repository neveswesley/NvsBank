using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Address.Commands.CreateAddress;

public class CreateAddressHandler : IRequestHandler<CreateAddressCommand, CreateAddressResponse>
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

    public async Task<CreateAddressResponse> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = _mapper.Map<Domain.Entities.Address>(request);

        await _addressRepository.CreateAsync(address);
        if (address.CustomerId == null)
            throw new ApplicationException("Customer not found.");

        var customer = await _unitOfWork.Customers.GetByIdAsync(address.CustomerId.Value);
        if (customer == null)
            throw new ApplicationException("Customer not found.");

        customer.AddressId = address.Id;
        
        _unitOfWork.Customers.UpdateAsync(customer);

        await _unitOfWork.Commit(cancellationToken);
        
        var response = _mapper.Map<CreateAddressResponse>(address);
        response.CustomerName = customer.FullName;

        return response;
    }
}