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
        [HttpPut("complete-registration")]
        public async Task<IActionResult> Complete(
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
        
        [Authorize(Roles = nameof(UserRole.Customer))]
        [HttpPut("update-customer")]
        public async Task<IActionResult> UpdateCustomer(
            [FromBody] UpdateCustomer.UpdateCustomerRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command =
                new UpdateCustomer.UpdateCustomerCommand(Guid.Parse(userId), request.FullName, request.PhoneNumber, request.Email);

            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        
    }
}