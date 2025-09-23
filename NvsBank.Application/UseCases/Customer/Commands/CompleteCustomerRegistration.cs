using MediatR;
using Microsoft.AspNetCore.Identity;
using NvsBank.Domain.Entities;
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
        private readonly UserManager<User> _userManager;


        public CompleteCustomerRegistrationHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _userManager = userManager;
        }

        public async Task<CustomerResponse> Handle(CompleteCustomerRegistrationCommand request,
            CancellationToken cancellationToken)
        {
            
            var user = _userManager.Users.SingleOrDefault(u => u.Id == request.Id);
            
            if (user == null)
                throw new ApplicationException("User not found");
            
            user.PhoneNumber = request.PhoneNumber;
            
            var customer = await _customerRepository.GetByIdAsync(user.PersonId);
            
            if (customer.IsActive(customer.Status) == false)
            {
                throw new ApplicationException("The user's account is not active.");
            }
            
            customer.CompleteRegistration(request.CustomerType, request.DocumentNumber, request.BirthDate,
                request.PhoneNumber);

            user.PhoneNumber = request.PhoneNumber;
            
            _customerRepository.UpdateAsync(customer);
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