using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Address.Commands.DeleteAddress;

public class DeleteAddressHandler : IRequestHandler<DeleteAddressCommand, DeleteAddressResponse>
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;

    public DeleteAddressHandler(IUnitOfWork unitOfWork, IAddressRepository addressRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _addressRepository = addressRepository;
        _mapper = mapper;
    }

    public async Task<DeleteAddressResponse> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var response = _mapper.Map<Domain.Entities.Address>(request);
       
        _addressRepository.DeleteAsync(response);

        await _unitOfWork.Commit(cancellationToken);
        
        return new DeleteAddressResponse("Address deleted successfully.");
    }
}