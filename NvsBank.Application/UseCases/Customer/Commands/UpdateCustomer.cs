using MediatR;
using Microsoft.AspNetCore.Identity;
using NvsBank.Application.Exceptions;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Commands;

public abstract class UpdateCustomer
{
    public class UpdateCustomerRequest : IRequest<UpdateCustomerCommand>
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
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
        private readonly UserManager<User> _userManager;

        public UpdateCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<CustomerResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == request.Id);
            if (user == null)
                throw new NotFoundException("User not found");

            var customer = await _customerRepository.GetByIdAsync(user.PersonId);

            if (customer == null)
                throw new NotFoundException($"User {request.Id} not found");
            
            user.FullName = request.FullName;
            user.Email = request.Email;

            customer.UpdateCustomer(request.FullName, request.PhoneNumber, request.Email);

            _customerRepository.UpdateAsync(customer);

            await _unitOfWork.Commit(cancellationToken);

            return new CustomerResponse
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
        }
    }
}