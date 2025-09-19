using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Commands;

public abstract class CreateCustomer
{
    public sealed record CreateCustomerCommand : IRequest<CustomerResponse>
    {
        public CustomerType Type { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public DateTime? FoundationDate { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerResponse> Handle(CreateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            
            var customer = new Domain.Entities.Customer
            {
                CustomerType = request.Type,
                DocumentNumber = request.DocumentNumber,
                BirthDate = request.BirthDate,
                PhoneNumber = request.PhoneNumber,
                Status = PersonStatus.Active
            };

            await _customerRepository.CreateAsync(customer);
            
            await _unitOfWork.Commit(cancellationToken);

            var response = new CustomerResponse
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Type = customer.CustomerType,
                DocumentNumber = customer.DocumentNumber,
                BirthDate = customer.BirthDate,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Status = customer.Status
            };
            
            return response;
        }
    }
}