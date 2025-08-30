using AutoMapper;
using NvsBank.Application.UseCases.Employee.Commands.CreateEmployee;

namespace NvsBank.Application.UseCases.Customer.Mapping;

public class CreateCustomerMapping : Profile
{
    public CreateCustomerMapping()
    {
        CreateMap<CreateCustomerCommand, Domain.Entities.Customer>().ReverseMap();
        CreateMap<CreateCustomerResponse, Domain.Entities.Customer>().ReverseMap();
    }
}