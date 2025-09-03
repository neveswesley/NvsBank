using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Queries.GetCustomerById;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, GetCustomerByIdResponse>
{
    
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerByIdHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<GetCustomerByIdResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _customerRepository.GetByIdAsync(request.Id);
        
        return _mapper.Map<GetCustomerByIdResponse>(response);
        
    }
}