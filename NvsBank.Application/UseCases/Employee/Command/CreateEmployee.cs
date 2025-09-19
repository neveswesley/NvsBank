using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Employee.Command;

public class CreateEmployee
{
    public sealed record CreateEmployeeCommand(string DocumentNumber, DateTime BirthDate, string PhoneNumber)
        : IRequest<EmployeeResponse>;

    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, EmployeeResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateEmployeeHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<EmployeeResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = new Domain.Entities.Employee
            {
                DocumentNumber = request.DocumentNumber,
                BirthDate = request.BirthDate,
                PhoneNumber = request.PhoneNumber
            };
            
            await _employeeRepository.CreateAsync(employee);

            var response = new EmployeeResponse
            {
                Id = employee.Id,
                FullName = employee.FullName,
                DocumentNumber = employee.DocumentNumber,
                BirthDate = employee.BirthDate,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                UserId = employee.UserId,
                Status = employee.Status
            };
            
            await _unitOfWork.Commit(cancellationToken);

            return response;
        }
    }
}