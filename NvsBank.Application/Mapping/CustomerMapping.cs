using AutoMapper;
using NvsBank.Application.UseCases.Customer.Commands;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Account.Mapping;

public class CustomerMapping : Profile
{
    public CustomerMapping()
    {
        CreateMap<Domain.Entities.Customer, CreateCustomer.CreateCustomerCommand>().ReverseMap();
        CreateMap<Domain.Entities.Customer, CustomerResponse>().ReverseMap();
    }
}