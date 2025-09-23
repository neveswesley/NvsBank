using MediatR;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Commands;

public class ActiveCustomer
{
    public sealed record ActiveCustomerCommand(Guid CustomerId) : IRequest<string>;
    
    public sealed record ActiveCustomerRequest : IRequest<ActiveCustomerCommand>;
    
    public class ActiveCustomerHandler : IRequestHandler<ActiveCustomerCommand, string>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActiveCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(ActiveCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

            if (customer.Status == PersonStatus.Active)
                throw new ApplicationException("The customer account is already active.");
            
            customer.Status = PersonStatus.Active;
            await _unitOfWork.Commit(cancellationToken);
            
            return "Customer account has been successfully activated.";
        }
    }
}