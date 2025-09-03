using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Address.Commands.DeleteAddress;

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
        var address = await _addressRepository.GetByIdAsync(request.Id);
        if (address == null)
            throw new ApplicationException("Address not found");

        var customer = await _unitOfWork.Customers.GetByIdAsync(address.CustomerId);
        if (customer != null)
        {
            customer.AddressId = null;
            customer.Address = null;
        }

        _addressRepository.DeleteAsync(address);
        await _unitOfWork.Commit(cancellationToken);

        return new DeleteAddressResponse("Address deleted successfully.");
    }
}