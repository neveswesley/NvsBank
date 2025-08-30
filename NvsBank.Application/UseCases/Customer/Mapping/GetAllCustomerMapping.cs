using AutoMapper;

namespace NvsBank.Application.UseCases.Customer.Queries.GetAllCustomer;

public class GetAllCustomerMapping : Profile
{
    public GetAllCustomerMapping()
    {
       CreateMap<Domain.Entities.Customer, GetAllCustomerResponse>().ReverseMap();
    }
}