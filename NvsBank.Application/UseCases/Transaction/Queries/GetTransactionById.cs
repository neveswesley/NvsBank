using MediatR;
using NvsBank.Domain.Entities.DTO;
using NvsBank.Infrastructure.Repositories;

namespace NvsBank.Application.UseCases.Transaction.Queries;

public abstract class GetTransactionById
{
    public sealed record GetTransactionByIdQuery(Guid AccountId, int Page, int PageSize) : IRequest<PagedResult<TransactionResponse>>;
    
    public class GetTransactionQueryHandler : IRequestHandler<GetTransactionByIdQuery, PagedResult<TransactionResponse>>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<PagedResult<TransactionResponse>> Handle(GetTransactionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetByAccountIdAsync(request.AccountId, request.Page, request.PageSize);
            
            if (transactions == null)
                throw new ApplicationException($"Account {request.AccountId} not found");

            var response = transactions.Items.Select(t => new TransactionResponse
            {
                TransactionId = t.Id,
                AccountId = t.AccountId,
                Amount = t.Amount,
                NewBalance = t.NewBalance,
                OldBalance = t.OldBalance,
                TransactionType = t.TransactionType.ToString(),
                Description = t.Description,
                Timestamp = t.Timestamp,
            }).ToList();
            
            return new PagedResult<TransactionResponse>
            {
                Items = response,
                Page = transactions.Page,
                PageSize = transactions.PageSize,
                TotalCount = transactions.TotalCount
            };
        }
    }
}