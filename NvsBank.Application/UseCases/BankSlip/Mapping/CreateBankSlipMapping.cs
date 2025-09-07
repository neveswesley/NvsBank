using AutoMapper;
using NvsBank.Application.UseCases.BankSlip.Command.CreateBankSlip;
using NvsBank.Application.UseCases.Transaction.Commands.CreateBankSlip;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.BankSlip.Mapping;

public class CreateBankSlipMapping : Profile
{
    public CreateBankSlipMapping()
    {
        CreateMap<Domain.Entities.BankSlip, BankSlipResponse>().ReverseMap();
    }
}