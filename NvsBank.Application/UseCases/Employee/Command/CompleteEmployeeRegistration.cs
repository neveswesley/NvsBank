using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Employee.Command;

public class CompleteEmployeeRegistration
{
    public sealed record CompleteEmployeeRegistrationCommand(Guid Id, 
        string DocumentNumber,
        DateTime BirthDate,
        string PhoneNumber) : IRequest<EmployeeResponse>;

    public sealed record CompleteEmployeeRegistrationRequest(
        string DocumentNumber,
        DateTime BirthDate,
        string PhoneNumber) : IRequest<CompleteEmployeeRegistrationCommand>;
    
    public class CompleteEmployeeRegistrationHandler : IRequestHandler<CompleteEmployeeRegistrationCommand, EmployeeResponse>
    {
        
        private readonly IEmployeeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteEmployeeRegistrationHandler(IEmployeeRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<EmployeeResponse> Handle(CompleteEmployeeRegistrationCommand request, CancellationToken cancellationToken)
        {
            var employeeCompleteRegistration = await _repository.GetByIdAsync(request.Id);
            
            if (employeeCompleteRegistration == null)
                throw new ApplicationException("Employee not found.");
            
            employeeCompleteRegistration.CompleteRegistrationEmployee(request.DocumentNumber, request.BirthDate, request.PhoneNumber);
            
            _repository.UpdateAsync(employeeCompleteRegistration);
            await _unitOfWork.Commit(cancellationToken);
            
            return new EmployeeResponse
            {
                Id = employeeCompleteRegistration.Id,
                FullName = employeeCompleteRegistration.FullName,
                DocumentNumber = employeeCompleteRegistration.DocumentNumber,
                BirthDate = employeeCompleteRegistration.BirthDate,
                Email = employeeCompleteRegistration.Email,
                PhoneNumber = employeeCompleteRegistration.PhoneNumber,
                UserId = employeeCompleteRegistration.UserId,
                Status = employeeCompleteRegistration.Status
            };
            
        }
    }
}