using MediatR;
using Microsoft.AspNetCore.Identity;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Auth.Commands
{
    public sealed record CreateEmployeeCommand(
        string FullName,
        string UserName,
        string Email,
        string Password,
        UserRole Role)
        : IRequest<Guid>;

    public sealed record CreateEmployeeResponse(string FullName, string UserName, string Email, UserRole Role);

    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, Guid>
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<User> _userManager;

        public CreateEmployeeHandler(IPasswordHasher<User> passwordHasher, IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository, IEmployeeRepository employeeRepository,
            UserManager<User> userManager)
        {
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
        }

        public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
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
                CreatedDate = DateTime.Now,
                UserId = user.Id
            };

            await _employeeRepository.CreateAsync(employee);
            user.PersonId = employee.Id;

            await _unitOfWork.Commit(cancellationToken);
            return user.Id;
        }
    }
}