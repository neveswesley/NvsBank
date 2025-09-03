using AutoMapper;
using MediatR;
using NvsBank.Application.Interfaces;

namespace NvsBank.Application.UseCases.Account.Queries.GetAllAccount;

public class GetAllAccountHandler : IRequestHandler<GetAllAccountQuery, List<GetAllAccountResponse>>
{
    
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllAccountHandler(IAccountRepository accountRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<GetAllAccountResponse>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetAllAsync(cancellationToken);

        return accounts.Select(x => new GetAllAccountResponse
        {
            Id = x.Id,
            AccountNumber = x.AccountNumber,
            Branch = x.Branch,
            AccountType = x.AccountType,
            Balance = x.Balance,
            OverdraftLimit = x.OverdraftLimit,
            OpeningDate = x.OpeningDate,
            ClosingDate = x.ClosingDate,
            Status = x.Status,
            CustomerId = x.CustomerId
        }).ToList();
    }
}