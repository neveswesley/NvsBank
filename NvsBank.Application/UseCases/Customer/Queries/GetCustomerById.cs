using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Queries;

public abstract class GetCustomerById
{
    public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<GetCustomerResponse>;

    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, GetCustomerResponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<GetCustomerResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);
            if (customer == null) throw new ApplicationException($"Customer {request.Id} not found");

            return new GetCustomerResponse
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Type = customer.Type,
                DocumentNumber = customer.DocumentNumber,
                BirthDate = customer.BirthDate,
                FoundationDate = customer.FoundationDate,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                CustomerStatus = customer.CustomerStatus,
                AddressStreet = customer.Address?.Street,
                AddressNumber = customer.Address?.Number,
                AddressCity = customer.Address?.City,
                AddressState = customer.Address?.State,
                AddressZipCode = customer.Address?.ZipCode,
                Accounts = customer.Account.Select(c => new AccountResponse
                {
                    Id = c.Id,
                    AccountNumber = c.AccountNumber,
                    Branch = c.Branch,
                    AccountType = c.AccountType,
                    Balance = c.Balance,
                    OverdraftLimit = c.OverdraftLimit,
                    OpeningDate = c.OpeningDate,
                    ClosingDate = c.ClosingDate,
                    Status = c.AccountStatus,
                    CustomerId = c.CustomerId
                })
            };
        }
    }
}