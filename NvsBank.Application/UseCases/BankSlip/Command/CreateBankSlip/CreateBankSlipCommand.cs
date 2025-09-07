using MediatR;
using NvsBank.Application.UseCases.BankSlip.Command.CreateBankSlip;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Transaction.Commands.CreateBankSlip;

public sealed record CreateBankSlipCommand : IRequest<BankSlipResponse>
{
    public decimal Amount { get; set; }
    public Guid PayeeId { get; set; }
    public Guid PayerId { get; set; }
}