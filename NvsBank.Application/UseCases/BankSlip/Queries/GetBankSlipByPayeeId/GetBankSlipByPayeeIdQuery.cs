using MediatR;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.BankSlip.Queries.GetBankSlipById;

public sealed record GetBankSlipByPayeeIdQuery (Guid PayeeId): IRequest<BankSlipResponse>;