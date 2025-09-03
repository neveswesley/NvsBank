using AutoMapper;
using NvsBank.Application.UseCases.Account.Commands.CreateAccount;
using NvsBank.Application.UseCases.Account.CreateAccount;

namespace NvsBank.Application.UseCases.Account.Mapping;

public class CreateAccountMapping : Profile
{
    public CreateAccountMapping()
    {
        CreateMap<CreateAccountCommand, Domain.Entities.Account>().ReverseMap();
        CreateMap<CreateAccountResponse, Domain.Entities.Account>().ReverseMap();
    }
}