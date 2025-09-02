using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Application.UseCases.Employee.Commands.CreateEmployee;
using NvsBank.Domain.Entities;

namespace NvsBank.Application.UseCases.Employee.Commands.CreateCustomer;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResponse>
{
    
    private readonly ICustomerRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerHandler(ICustomerRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = _mapper.Map<Domain.Entities.Customer>(request);
        
        var exists = await _userRepository.ExistsByEmailAsync(request.Email);
        if (exists)
            throw new ApplicationException("This email already exists");
        
        await _userRepository.CreateAsync(customer);

        await _unitOfWork.Commit(cancellationToken);

        return _mapper.Map<CreateCustomerResponse>(customer);

    }
}