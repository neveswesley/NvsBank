using AutoMapper;
using NvsBank.Application.UseCases.Customer.Queries.GetCustomerByDocument;

namespace NvsBank.Application.UseCases.Customer.Mapping;

public class GetCustomerByDocumentMapping : Profile
{
    public GetCustomerByDocumentMapping()
    {
        CreateMap<Domain.Entities.Customer, GetCustomerByDocumentResponse>();
    }
}