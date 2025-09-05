using MediatR;
using NvsBank.Domain.Entities.DTO;

namespace NvsBank.Application.UseCases.Transaction.Commands.Transfer;

public sealed record TransferCommand (Guid To, Guid From, decimal Amount, string Description) : IRequest<TransferResponse>;