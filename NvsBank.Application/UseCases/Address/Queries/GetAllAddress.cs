using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Address.Queries;

public abstract class GetAllAddress
{
    public sealed record GetAllAddressQuery : IRequest<List<AddressResponse>>;
    
    public class GetAllAddressHandler : IRequestHandler<GetAllAddressQuery, List<AddressResponse>>
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


        public async Task<List<AddressResponse>> Handle(GetAllAddressQuery request,
            CancellationToken cancellationToken)
        {
            var response = await _addressRepository.GetAllWithCustomerAsync();

            if (response == null)
                throw new ApplicationException("Address not found.");

            return response.Select(x => new AddressResponse
            {
                Id = x.Id,
                Street = x.Street,
                Number = x.Number,
                City = x.City,
                State = x.State,
                ZipCode = x.ZipCode,
                CustomerId = x.Customer.Id
            }).ToList();
        }
    }
    
}