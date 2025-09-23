using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NvsBank.Application.UseCases.Customer.Commands;
using NvsBank.Application.UseCases.Customer.Queries;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = nameof(UserRole.Customer))]
        [HttpPut("complete-my-registration")]
        public async Task<IActionResult> CompleteMyRegistration(
            [FromBody] CompleteCustomerRegistration.CompleteCustomerRegistrationRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new CompleteCustomerRegistration.CompleteCustomerRegistrationCommand(
                Guid.Parse(userId), request.CustomerType, request.DocumentNumber, request.BirthDate, request.PhoneNumber
            );

            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("complete-registration/{id}")]
        public async Task<IActionResult> Complete(Guid id,
            [FromBody] CompleteCustomerRegistration.CompleteCustomerRegistrationRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CompleteCustomerRegistration.CompleteCustomerRegistrationCommand(
                id, request.CustomerType, request.DocumentNumber, request.BirthDate, request.PhoneNumber
            );

            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        
        [Authorize(Roles = nameof(UserRole.Customer))]
        [HttpPut("update-my-customer")]
        public async Task<IActionResult> UpdateMyCustomer(
            [FromBody] UpdateCustomer.UpdateCustomerRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command =
                new UpdateCustomer.UpdateCustomerCommand(Guid.Parse(userId), request.FullName, request.PhoneNumber, request.Email);

            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("update-customer/{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, 
            [FromBody] UpdateCustomer.UpdateCustomerRequest request,
            CancellationToken cancellationToken)
        {
            var command =
                new UpdateCustomer.UpdateCustomerCommand(id, request.FullName, request.PhoneNumber, request.Email);

            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("update-customer-status/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id,
            [FromBody] UpdateCustomerStatus.UpdateCustomerStatusRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateCustomerStatus.ChangeCustomerStatusCommand(id, request.Status, request.Reason);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("active-customer/{id}")]
        public async Task<IActionResult> ActiveCustomer(Guid id,
            [FromBody] ActiveCustomer.ActiveCustomerRequest request, CancellationToken cancellationToken)
        {
            var command = new ActiveCustomer.ActiveCustomerCommand(id);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        
        [Authorize(Roles = nameof(UserRole.Teller) + "," + nameof(UserRole.Analyst) + "," + nameof(UserRole.Admin))]
        [HttpGet("get-all-customers")]
        public async Task<IActionResult> GetAllCustomers(int page, int pageSize)
        {
            var response = await _mediator.Send(new GetAllCustomer.GetAllCustomerQuery(page, pageSize));
            return Ok(response);
        }
        
        [Authorize(Roles = nameof(UserRole.Teller) + "," + nameof(UserRole.Analyst) + "," + nameof(UserRole.Admin))]
        [HttpGet("get-customer-by-id/{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerById.GetCustomerByIdQuery(id));

            return Ok(customer);
        }
        
        [Authorize(Roles = nameof(UserRole.Teller) + "," + nameof(UserRole.Analyst) + "," + nameof(UserRole.Admin))]
        [HttpGet("get-customer-by-documentNumber/{documentNumber}")]
        public async Task<IActionResult> GetCustomerByDocumentNumber(string documentNumber)
        {
            var customer = await _mediator.Send(new GetCustomerByDocument.GetCustomerByDocumentQuery(documentNumber));

            return Ok(customer);
        }
        
    }
}