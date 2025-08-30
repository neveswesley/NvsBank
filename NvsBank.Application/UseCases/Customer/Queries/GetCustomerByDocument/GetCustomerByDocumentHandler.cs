using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Queries.GetCustomerByDocument;

public class GetCustomerByDocumentHandler : IRequestHandler<GetCustomerByDocumentQuery, GetCustomerByDocumentResponse>
{
    
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerByDocumentHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<GetCustomerByDocumentResponse> Handle(GetCustomerByDocumentQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByDocument(request.Document);
        return _mapper.Map<GetCustomerByDocumentResponse>(customer);
    }
}