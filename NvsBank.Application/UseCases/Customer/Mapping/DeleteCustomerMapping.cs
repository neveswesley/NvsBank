using AutoMapper;
using NvsBank.Application.UseCases.Customer.Commands.DeleteCustomer;

namespace NvsBank.Application.UseCases.Customer.Mapping;

public class DeleteCustomerMapping : Profile
{
    public DeleteCustomerMapping()
    {
        CreateMap<Domain.Entities.Customer, DeleteCustomerCommand>().ReverseMap();
    }
}