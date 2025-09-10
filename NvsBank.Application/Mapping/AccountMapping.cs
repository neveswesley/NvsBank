using AutoMapper;
using NvsBank.Application.UseCases.Account.Commands;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Account.Mapping;

public class AccountMapping : Profile
{
    public AccountMapping()
    {
        CreateMap<List<Domain.Entities.Account>, AccountResponse>().ReverseMap();
        CreateMap<CreateAccountRequest, Domain.Entities.Account>().ReverseMap();
        CreateMap<AccountResponse, Domain.Entities.Account>().ReverseMap();
    }
}