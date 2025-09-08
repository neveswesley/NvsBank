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
        
        [HttpPost("")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomer.CreateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var response = await _mediator.Send(new GetAllCustomer.GetAllCustomerQuery());
            return Ok(response);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerById.GetCustomerByIdQuery(id));
            
            return Ok(customer);
        }

        [HttpGet("document/{documentNumber}")]
        public async Task<IActionResult> GetCustomerByDocumentNumber(string documentNumber)
        {
            var customer = await _mediator.Send(new GetCustomerByDocument.GetCustomerByDocumentQuery(documentNumber));
            
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomer.UpdateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            if (id != request.Id)
                return BadRequest("Ids do not match");
            
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new DeleteCustomer.DeleteCustomerCommand(id), cancellationToken);
            return Ok(response);
        }
        
    }
}
