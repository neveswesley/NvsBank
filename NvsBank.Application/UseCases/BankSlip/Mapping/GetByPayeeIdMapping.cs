using AutoMapper;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.BankSlip.Mapping;

public class GetByPayeeIdMapping : Profile
{
    public GetByPayeeIdMapping()
    {
        CreateMap<Domain.Entities.BankSlip, BankSlipResponse>();
    }
}