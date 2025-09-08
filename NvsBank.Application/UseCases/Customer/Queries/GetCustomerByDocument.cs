using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Customer.Queries;

public abstract class GetCustomerByDocument
{
    public sealed record GetCustomerByDocumentQuery (string Document) : IRequest<GetCustomerResponse>;
    
    public class GetCustomerByDocumentHandler : IRequestHandler<GetCustomerByDocumentQuery, GetCustomerResponse>
    {
        
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerByDocumentHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<GetCustomerResponse> Handle(GetCustomerByDocumentQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByDocumentWithAddressAsync(request.Document);
            return _mapper.Map<GetCustomerResponse>(customer);
        }
    }
}