using MediatR;
using Microsoft.AspNetCore.Identity;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Auth.Commands
{
    public record CreateUserCommand(string FullName, string UserName, string Email, string Password, UserRole Role)
        : IRequest<Guid>;

    public sealed record CreateUserResponse(string FullName, string UserName, string Email, UserRole Role);

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
                Role = request.Role
            };
            
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new ApplicationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            if (request.Role == UserRole.Customer)
            {
                var customer = new Domain.Entities.Customer
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Status = PersonStatus.Active,
                    Limits = new OperationLimits
                    {
                        CustomerSingleTransferLimit = 5000,
                        CustomerDailyTransferLimit = 20000,
                        CustomerSinglePaymentLimit = 1000,
                        CustomerDailyPaymentLimit = 5000
                    },
                    UserId = user.Id
                };

                await _customerRepository.CreateAsync(customer);
                user.PersonId = customer.Id;
            }
            else
            {
                var employee = new Domain.Entities.Employee
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Status = PersonStatus.Active,
                    Limits = request.Role switch
                    {
                        UserRole.Admin => new OperationLimits { CanOverrideLimits = true },
                        UserRole.Analyst => new OperationLimits { ApprovalLimit = 100000, MaxPendingApprovals = 5 },
                        UserRole.Teller => new OperationLimits
                        {
                            TellerSingleWithdrawalLimit = 10000,
                            TellerDailyWithdrawalLimit = 50000,
                            TellerSingleDepositLimit = 20000,
                            TellerDailyDepositLimit = 80000
                        },
                        _ => new OperationLimits()
                    },
                    UserId = user.Id
                };

                await _employeeRepository.CreateAsync(employee);
                user.PersonId = employee.Id;

            }
            
            

            await _unitOfWork.Commit(cancellationToken);
            return user.Id;
        }
    }
}