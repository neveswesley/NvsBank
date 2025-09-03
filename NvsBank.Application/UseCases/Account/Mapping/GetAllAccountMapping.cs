using AutoMapper;
using NvsBank.Application.UseCases.Account.Queries.GetAllAccount;

namespace NvsBank.Application.UseCases.Account.Mapping;

public class GetAllAccountMapping : Profile
{
    public GetAllAccountMapping()
    {
        CreateMap<List<Domain.Entities.Account>, GetAllAccountResponse>().ReverseMap();
    }
}