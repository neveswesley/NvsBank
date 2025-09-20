using MediatR;
using Microsoft.AspNetCore.Mvc;
using NvsBank.Application.UseCases.Customer.Commands;
using NvsBank.Application.UseCases.Customer.Queries;

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

        [HttpPut("complete-registration/{id}")]
        public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteCustomerRegistration.CompleteCustomerRegistrationRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new CompleteCustomerRegistration.CompleteCustomerRegistrationCommand(id, request.CustomerType, request.DocumentNumber, request.BirthDate, request.PhoneNumber), cancellationToken);
            return Ok(response);
        }

        [HttpGet("get-all-customers")]
        public async Task<IActionResult> GetAllCustomers(int page, int pageSize)
        {
            var response = await _mediator.Send(new GetAllCustomer.GetAllCustomerQuery(page, pageSize));
            return Ok(response);
        }

        [HttpGet("get-customer-by-id/{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerById.GetCustomerByIdQuery(id));

            return Ok(customer);
        }

        [HttpGet("get-customer-by-documentNumber/{documentNumber}")]
        public async Task<IActionResult> GetCustomerByDocumentNumber(string documentNumber)
        {
            var customer = await _mediator.Send(new GetCustomerByDocument.GetCustomerByDocumentQuery(documentNumber));

            return Ok(customer);
        }

        [HttpPut("update-customer/{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id,
            [FromBody] UpdateCustomer.UpdateCustomerRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateCustomer.UpdateCustomerCommand(id, request.FullName, request.PhoneNumber, request.Email);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPut("update-customer-status/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id,
            [FromBody] UpdateCustomerStatus.UpdateCustomerStatusRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateCustomerStatus.ChangeCustomerStatusCommand(id, request.Status, request.Reason);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
    }
}