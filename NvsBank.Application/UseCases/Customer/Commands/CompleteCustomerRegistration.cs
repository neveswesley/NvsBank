using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Commands;

public class CompleteCustomerRegistration
{
    public sealed record CompleteCustomerRegistrationCommand(
        Guid Id,
        CustomerType CustomerType,
        string DocumentNumber,
        DateTime BirthDate,
        string PhoneNumber) : IRequest<CustomerResponse>;

    public sealed record CompleteCustomerRegistrationRequest(
        CustomerType CustomerType,
        string DocumentNumber,
        DateTime BirthDate,
        string PhoneNumber) : IRequest<CompleteCustomerRegistrationCommand>;

    public class
        CompleteCustomerRegistrationHandler : IRequestHandler<CompleteCustomerRegistrationCommand, CustomerResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;

        public CompleteCustomerRegistrationHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponse> Handle(CompleteCustomerRegistrationCommand request,
            CancellationToken cancellationToken)
        {
            var employee = await _customerRepository.GetByIdAsync(request.Id);

            employee.CompleteRegistration(request.CustomerType, request.DocumentNumber, request.BirthDate,
                request.PhoneNumber);
            _customerRepository.UpdateAsync(employee);
            _unitOfWork.Commit(cancellationToken);

            var response = new CustomerResponse
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Type = employee.CustomerType,
                DocumentNumber = employee.DocumentNumber,
                BirthDate = employee.BirthDate,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                Status = employee.Status
            };
            
            return response;
        }
    }
}