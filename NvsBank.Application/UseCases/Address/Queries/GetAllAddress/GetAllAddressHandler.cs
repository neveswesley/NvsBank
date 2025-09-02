using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Application.UseCases.Address.Queries.GetAllAddress;
using NvsBank.Application.UseCases.Customer.Queries.GetAllCustomer;

namespace NvsBank.Application.UseCases.Address.Queries;

public class GetAllAddressHandler : IRequestHandler<GetAllAddressQuery, List<GetAllAddressResponse>>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllAddressHandler(IAddressRepository addressRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<List<GetAllAddressResponse>> Handle(GetAllAddressQuery request,
        CancellationToken cancellationToken)
    {
        var response = await _addressRepository.GetAllWithCustomerAsync();

        if (response == null)
            throw new ApplicationException("Address not found.");

        return response.Select(x => new GetAllAddressResponse
        {
            Id = x.Id,
            Street = x.Street,
            Number = x.Number,
            City = x.City,
            State = x.State,
            ZipCode = x.ZipCode,
            CustomerName = x.Customer?.FullName
        }).ToList();
    }
}