using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Address.Queries.GetAddressById;

public class GetAddressByIdHandler : IRequestHandler<GetAddressByIdQuery, GetAddressByIdResponse>
{
    
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;

    public GetAddressByIdHandler(IAddressRepository addressRepository, IMapper mapper)
    {
        _addressRepository = addressRepository;
        _mapper = mapper;
    }

    public async Task<GetAddressByIdResponse> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _addressRepository.GetByIdWithCustomerAsync(request.Id);
        if (response == null)
            throw new ApplicationException("Address not found");

        return new GetAddressByIdResponse
        {
            Id = response.Id,
            Street = response.Street,
            Number = response.Number,
            City = response.City,
            State = response.State,
            ZipCode = response.ZipCode,
            CustomerName = response.Customer?.FullName
        };
    }
}