using MediatR;
using Microsoft.AspNetCore.Identity;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Auth.Commands
{
    public record CreateUserCommand(string FullName, string UserName, string Email, string Password)
        : IRequest<Guid>;

    public sealed record CreateUserResponse(string FullName, string UserName, string Email);

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<User> _userManager;

        public CreateUserHandler(IPasswordHasher<User> passwordHasher, IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository,
            UserManager<User> userManager)
        {
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                FullName = request.FullName,
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = request.Password, 
                Role = UserRole.Customer
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new ApplicationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);


            var customer = new Domain.Entities.Customer
            {
                FullName = request.FullName,
                Email = request.Email,
                Status = PersonStatus.Pending,
                CustomerType = CustomerType.Individual,
                Limits = new OperationLimits
                {
                    CustomerSingleTransferLimit = 5000,
                    CustomerDailyTransferLimit = 20000,
                    CustomerSinglePaymentLimit = 1000,
                    CustomerDailyPaymentLimit = 5000
                },
                CreatedDate = DateTime.Now,
                UserId = user.Id
            };
            await _customerRepository.CreateAsync(customer);
            user.PersonId = customer.Id;

            await _unitOfWork.Commit(cancellationToken);
            return user.Id;
        }
    }
}