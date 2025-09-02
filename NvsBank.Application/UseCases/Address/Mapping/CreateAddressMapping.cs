using AutoMapper;
using NvsBank.Application.UseCases.Address.Commands.CreateAddress;
using NvsBank.Application.UseCases.Employee.Commands.CreateEmployee;

namespace NvsBank.Application.UseCases.Address.Mapping;

public class CreateAddressMapping : Profile
{
    public CreateAddressMapping()
    {
        CreateMap<CreateAddressCommand, Domain.Entities.Address>().ReverseMap();
        CreateMap<CreateAddressResponse, Domain.Entities.Address>().ReverseMap();
        CreateMap<CreateAddressResponse, CreateAddressCommand>().ReverseMap();
    }
}