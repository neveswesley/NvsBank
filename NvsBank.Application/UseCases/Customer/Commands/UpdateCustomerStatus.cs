using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Commands;

public abstract class UpdateCustomerStatus
{
    public class UpdateCustomerStatusRequest
    {
        public PersonStatus Status { get; set; }
        public string? Reason { get; set; }
    }

    public sealed record ChangeCustomerStatusCommand(Guid CustomerId, PersonStatus Status, string? Reason)
        : IRequest<CustomerStatusResponse>;

    public class UpdateCustomerStatusHandler : IRequestHandler<ChangeCustomerStatusCommand, CustomerStatusResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerStatusHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerStatusResponse> Handle(ChangeCustomerStatusCommand request,
            CancellationToken cancellationToken)
        {
            var customer = _customerRepository.GetByIdAsync(request.CustomerId).Result;
            if (customer == null)
                throw new ApplicationException("Customer not found.");
            
            var oldStatus = customer.Status;

            if (oldStatus == PersonStatus.Closed && request.Status == PersonStatus.Active)
                throw new ApplicationException("Closed customer cannot be reactivated.");

            customer.Status = request.Status;
            customer.StatusReason = request.Reason;
            customer.ModifiedDate = DateTime.Now;

            _customerRepository.UpdateAsync(customer);
            await _unitOfWork.Commit(cancellationToken);

            return new CustomerStatusResponse
            {
                CustomerId = customer.Id,
                OldStatus = oldStatus,
                NewStatus = customer.Status,
                Reason = customer.StatusReason,
                ChangedAt = customer.ModifiedDate
            };
        }
    }
}