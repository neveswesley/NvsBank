using AutoMapper;
using NvsBank.Application.UseCases.Address.Commands;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.Mapping;

public class AddressMapping : Profile
{
    public AddressMapping()
    {
        CreateMap<CreateAddress.CreateAddressCommand, Domain.Entities.Address>().ReverseMap();
        CreateMap<AddressResponse, Domain.Entities.Address>().ReverseMap();
        CreateMap<Domain.Entities.Address, DeleteAddress.DeleteAddressCommand>().ReverseMap();
    }
}