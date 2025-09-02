using AutoMapper;
using NvsBank.Application.UseCases.Address.Queries.GetAllAddress;

namespace NvsBank.Application.UseCases.Address.Mapping;

public class GetAllAddressMapping : Profile
{
    public GetAllAddressMapping()
    {
        CreateMap<GetAllAddressResponse, Domain.Entities.Address>().ReverseMap();
    }
}