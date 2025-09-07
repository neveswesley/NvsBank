using MediatR;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.BankSlip.Queries.GetBankSlipById;

public sealed record GetBankSlipByPayerIdQuery (Guid PayerId) : IRequest<BankSlipResponse>;