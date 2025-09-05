using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Application.UseCases.Account.Queries.GetAllAccount;
using NvsBank.Application.UseCases.Customer.Queries.GetAllCustomer;

namespace NvsBank.Application.UseCases.Customer.Queries.GetCustomerById;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, GetAllCustomerResponse>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<GetAllCustomerResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer == null) throw new ApplicationException($"Customer {request.Id} not found");

        return new GetAllCustomerResponse
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
            Accounts = customer.Account.Select(c => new GetAllAccountResponse
            {
                Id = c.Id,
                AccountNumber = c.AccountNumber,
                Branch = c.Branch,
                AccountType = c.AccountType,
                Balance = c.Balance,
                OverdraftLimit = c.OverdraftLimit,
                OpeningDate = c.OpeningDate,
                ClosingDate = c.ClosingDate,
                Status = c.Status,
                CustomerId = c.CustomerId
            })
        };
    }
}