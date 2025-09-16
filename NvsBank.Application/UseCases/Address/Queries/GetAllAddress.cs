using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Address.Queries;

public abstract class GetAllAddress
{
    public sealed record GetAllAddressQuery (int Page, int PageSize) : IRequest<PagedResult<AddressResponse>>;
    
    public class GetAllAddressHandler : IRequestHandler<GetAllAddressQuery, PagedResult<AddressResponse>>
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


        public async Task<PagedResult<AddressResponse>> Handle(GetAllAddressQuery request,
            CancellationToken cancellationToken)
        {
            var addressPaged = await _addressRepository.GetAllWithCustomerAsync(request.Page, request.PageSize);

            if (addressPaged == null)
                throw new ApplicationException("Address not found.");

            var addressResponse = addressPaged.Items.Select(x => new AddressResponse
            {
                Id = x.Id,
                Street = x.Street,
                Number = x.Number,
                City = x.City,
                State = x.State,
                ZipCode = x.ZipCode,
                CustomerId = x.Customer.Id
            }).ToList();
            
            return new PagedResult<AddressResponse>
            {
                Items = addressResponse,
                Page = addressPaged.Page,
                PageSize = addressPaged.PageSize,
                TotalCount = addressPaged.TotalCount
            };
        }
    }
    
}