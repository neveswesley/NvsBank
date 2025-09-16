using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.PixKey.Queries;

public sealed record GetPixKeysByIdQuery(Guid AccountId, int Page, int PageSize)
    : IRequest<PagedResult<PixKeyByIdResponse>>;

public class GetPixKeysByIdHandler(IPixKeyRepository pixKeyRepository)
    : IRequestHandler<GetPixKeysByIdQuery, PagedResult<PixKeyByIdResponse>>
{
    public async Task<PagedResult<PixKeyByIdResponse>> Handle(GetPixKeysByIdQuery request,
        CancellationToken cancellationToken)
    {
        var pixKeys = await pixKeyRepository.GetPixKeysByIdAsync(request.AccountId, request.Page, request.PageSize);
        var response = pixKeys.Items.Select(x => new PixKeyByIdResponse
        {
            KeyType = x.KeyType,
            KeyValue = x.KeyValue,
            Status = x.Status,
        });

        return new PagedResult<PixKeyByIdResponse>
        {
            Items = response,
            Page = pixKeys.Page,
            PageSize = pixKeys.PageSize,
            TotalCount = pixKeys.TotalCount
        };
    }
}