using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Address.Queries;

public abstract class GetAddressById
{
    public sealed record GetAddressByIdQuery(Guid Id) : IRequest<AddressResponse>;

    public class GetAddressByIdHandler : IRequestHandler<GetAddressByIdQuery, AddressResponse>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public GetAddressByIdHandler(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<AddressResponse> Handle(GetAddressByIdQuery request,
            CancellationToken cancellationToken)
        {
            var response = await _addressRepository.GetByIdWithCustomerAsync(request.Id);
            if (response == null)
                throw new ApplicationException("Address not found");

            return new AddressResponse
            {
                Id = response.Id,
                Street = response.Street,
                Number = response.Number,
                City = response.City,
                State = response.State,
                ZipCode = response.ZipCode,
                CustomerId = response.Customer.Id
            };
        }
    }
}