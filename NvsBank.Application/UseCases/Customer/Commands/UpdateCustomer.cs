using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Customer.Commands;

public abstract class UpdateCustomer
{

    public class UpdateCustomerRequest : IRequest<UpdateCustomerCommand>
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
    public sealed record UpdateCustomerCommand(
        Guid Id,
        string FullName,
        string PhoneNumber,
        string Email) : IRequest<CustomerResponse>;
    
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, CustomerResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);

            if (customer == null)
                throw new KeyNotFoundException($"User {request.Id} not found");
        
            customer.UpdateCustomer(request.FullName, request.PhoneNumber, request.Email);
        
            _customerRepository.UpdateAsync(customer);
        
            await _unitOfWork.Commit(cancellationToken);

            return new CustomerResponse
            {
                Id = customer.Id,
                FullName = customer.FullName,
                CustomerType = customer.Type,
                DocumentNumber = customer.DocumentNumber,
                BirthDate = customer.BirthDate,
                FoudationDate = customer.FoundationDate,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                CustomerStatus = customer.CustomerStatus
            };

        }
    }
}