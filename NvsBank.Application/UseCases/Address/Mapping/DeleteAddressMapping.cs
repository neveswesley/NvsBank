using AutoMapper;
using NvsBank.Application.UseCases.Address.Commands.DeleteAddress;

namespace NvsBank.Application.UseCases.Address.Mapping;

public class DeleteAddressMapping : Profile
{
    public DeleteAddressMapping()
    {
        CreateMap<Domain.Entities.Address, DeleteAddressCommand>().ReverseMap();
    }
}