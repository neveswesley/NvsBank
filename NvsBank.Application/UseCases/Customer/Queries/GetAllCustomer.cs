using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Customer.Queries;

public abstract class GetAllCustomer
{
    public sealed record GetAllCustomerQuery (int Page, int PageSize) : IRequest<PagedResult<GetCustomerResponse>>;
    
    public class GetAllCustomerHandler : IRequestHandler<GetAllCustomerQuery, PagedResult<GetCustomerResponse>>
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
    
    
        public async Task<PagedResult<GetCustomerResponse>> Handle(GetAllCustomerQuery request,
            CancellationToken cancellationToken)
        {
            var customers = await _unitOfWork.Customers.GetAllWithAddressAsync(request.Page, request.PageSize);
    
            var response = customers.Items.Select(x => new GetCustomerResponse
            {
                Id = x.Id,
                FullName = x.FullName,
                Type = x.CustomerType,
                DocumentNumber = x.DocumentNumber,
                BirthDate = x.BirthDate,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                CustomerStatus = x.Status,
                AddressStreet = x.Address?.Street,
                AddressNumber = x.Address?.Number,
                AddressCity = x.Address?.City,
                AddressState = x.Address?.State,
                AddressZipCode = x.Address?.ZipCode,
                Accounts = x.Accounts.Select(c => new AccountResponse
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
            }).ToList();
            
            return new PagedResult<GetCustomerResponse>
            {
                Items = response,
                Page = customers.Page,
                PageSize = customers.PageSize,
                TotalCount = customers.TotalCount
            };
        }
    }
    
}