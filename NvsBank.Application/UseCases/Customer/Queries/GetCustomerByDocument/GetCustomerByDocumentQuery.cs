using MediatR;
using NvsBank.Application.UseCases.Customer.Queries.GetCustomerById;

namespace NvsBank.Application.UseCases.Customer.Queries.GetCustomerByDocument;

public sealed record GetCustomerByDocumentQuery (string Document) : IRequest<GetCustomerByDocumentResponse>;