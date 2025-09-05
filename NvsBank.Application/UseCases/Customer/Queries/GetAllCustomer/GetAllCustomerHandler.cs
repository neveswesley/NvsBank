using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Application.UseCases.Account.Queries.GetAllAccount;

namespace NvsBank.Application.UseCases.Customer.Queries.GetAllCustomer;

public class GetAllCustomerHandler : IRequestHandler<GetAllCustomerQuery, List<GetAllCustomerResponse>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCustomerHandler(ICustomerRepository customerRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<List<GetAllCustomerResponse>> Handle(GetAllCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var customers = await _unitOfWork.Customers.GetAllWithAddressAsync();

        var response = customers.Select(x => new GetAllCustomerResponse
        {
            Id = x.Id,
            FullName = x.FullName,
            Type = x.Type,
            DocumentNumber = x.DocumentNumber,
            BirthDate = x.BirthDate,
            FoundationDate = x.FoundationDate,
            PhoneNumber = x.PhoneNumber,
            Email = x.Email,
            CustomerStatus = x.CustomerStatus,
            AddressStreet = x.Address?.Street,
            AddressNumber = x.Address?.Number,
            AddressCity = x.Address?.City,
            AddressState = x.Address?.State,
            AddressZipCode = x.Address?.ZipCode,
            Accounts = x.Account.Select(c => new GetAllAccountResponse
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
        }).ToList();
        return response;
    }
}