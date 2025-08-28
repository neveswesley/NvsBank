using AutoMapper;
using NvsBank.Application.UseCases.Employee.Commands.CreateEmployee;

namespace NvsBank.Application.UseCases.Customer.Mapping;

public class CreateUserMapping : Profile
{
    public CreateUserMapping()
    {
        CreateMap<CreateCustomerCommand, Domain.Entities.Customer>().ReverseMap();
        CreateMap<CreateCustomerResponse, Domain.Entities.Customer>().ReverseMap();
    }
}