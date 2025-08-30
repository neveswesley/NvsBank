using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Queries.GetAllCustomer;

public class GetAllCustomerHandler : IRequestHandler<GetAllCustomerQuery, List<GetAllCustomerResponse>>
{
    
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetAllCustomerHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<List<GetAllCustomerResponse>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
    {
        var response = await _customerRepository.GetAllAsync(cancellationToken);

        if (response == null)
            throw new ApplicationException("Customer not found.");

        return response.Select(_mapper.Map<GetAllCustomerResponse>).ToList();
    }
}