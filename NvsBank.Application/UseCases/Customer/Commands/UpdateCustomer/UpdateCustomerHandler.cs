using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Commands.UpdateCustomer;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResponse>
{
    
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCustomerResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);

        if (customer == null)
            throw new KeyNotFoundException($"User {request.Id} not found");
        
        customer.UpdateCustomer(request.FullName, request.Type, request.DocumentNumber, request.BirthDate, request.FoundationDate, request. PhoneNumber, request.Email, request.CustomerStatus);
        
        _customerRepository.UpdateAsync(customer);
        
        await _unitOfWork.Commit(cancellationToken);

        return new UpdateCustomerResponse(customer.Id, customer.FullName, customer.Type, customer.DocumentNumber, customer.BirthDate, customer.FoundationDate, customer.PhoneNumber, customer.Email, customer.CustomerStatus);

    }
}