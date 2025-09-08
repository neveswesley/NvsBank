using AutoMapper;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.Mapping;

public class BankSlipMapping : Profile
{
    public BankSlipMapping()
    {
        CreateMap<Domain.Entities.BankSlip, BankSlipResponse>().ReverseMap();
        CreateMap<Domain.Entities.BankSlip, BankSlipResponse>();
    }
}