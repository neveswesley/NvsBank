using AutoMapper;
using NvsBank.Application.UseCases.Customer.Commands.UpdateCustomer;

namespace NvsBank.Application.UseCases.Customer.Mapping;

public class UpdateCustomerMapping : Profile
{
    public UpdateCustomerMapping()
    {
        CreateMap<Domain.Entities.Customer, UpdateCustomerResponse>().ReverseMap();
        CreateMap<Domain.Entities.Customer, UpdateCustomerCommand>().ReverseMap();
    }
}