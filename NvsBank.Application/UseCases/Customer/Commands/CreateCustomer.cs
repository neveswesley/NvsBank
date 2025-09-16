using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Commands;

public abstract class CreateCustomer
{
    public sealed record CreateCustomerCommand : IRequest<CustomerResponse>
    {
        public string FullName { get; set; } = string.Empty;
        public CustomerType Type { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public DateTime? FoundationDate { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public CustomerStatus CustomerStatus { get; set; }
    }

    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerResponse>
    {
        private readonly ICustomerRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerHandler(ICustomerRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerResponse> Handle(CreateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Domain.Entities.Customer>(request);

            var exists = await _userRepository.ExistsByEmailAsync(request.Email);
            if (exists)
                throw new ApplicationException("This email already exists");

            await _userRepository.CreateAsync(customer);

            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<CustomerResponse>(customer);
        }
    }
}