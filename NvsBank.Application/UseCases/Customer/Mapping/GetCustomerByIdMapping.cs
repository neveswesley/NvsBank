using AutoMapper;

namespace NvsBank.Application.UseCases.Customer.Queries.GetCustomerById;

public class GetCustomerByIdMapping : Profile
{
    public GetCustomerByIdMapping()
    {
        CreateMap<GetCustomerByIdResponse, Domain.Entities.Customer>().ReverseMap();
    }
}