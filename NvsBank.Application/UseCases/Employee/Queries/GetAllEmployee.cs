using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Employee.Queries;

public class GetAllEmployee
{
    public sealed record GetAllEmployeeQuery(int Page, int PageSize) : IRequest<PagedResult<EmployeeResponse>>;

    public class GetAllEmployeeHandler : IRequestHandler<GetAllEmployeeQuery, PagedResult<EmployeeResponse>>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetAllEmployeeHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<PagedResult<EmployeeResponse>> Handle(GetAllEmployeeQuery request,
            CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetPagedAsync(request.Page, request.PageSize);

            var employeeResponse = employees.Items.Select(x => new EmployeeResponse
            {
                Id = x.Id,
                FullName = x.FullName,
                DocumentNumber = x.DocumentNumber,
                BirthDate = x.BirthDate,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                UserId = x.UserId,
            });

            return new PagedResult<EmployeeResponse>
            {
                Items = employeeResponse,
                Page = employees.Page,
                PageSize = employees.PageSize,
                TotalCount = employees.TotalCount
            };
        }
    }
}