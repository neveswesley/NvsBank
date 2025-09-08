using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Commands;

public abstract class DeleteCustomer
{
    public sealed record DeleteCustomerCommand(Guid Id) : IRequest<DeleteCustomerResponse>;
    
    public sealed record DeleteCustomerResponse(string Message);

    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, DeleteCustomerResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteCustomerResponse> Handle(DeleteCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var account = _customerRepository.GetByIdAsync(request.Id).Result;
            _customerRepository.DeleteAsync(account);
            await _unitOfWork.Commit(cancellationToken);
            return new DeleteCustomerResponse("Account has been deleted.");
        }
    }
}