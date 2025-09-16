using FluentValidation;
using NvsBank.Application.UseCases.Transaction.Commands;

namespace NvsBank.Application.UseCases.Transaction.Validators;

public class PixTransferValidator : AbstractValidator<Transfer.TransferCommand>
{
    public PixTransferValidator()
    {
        RuleFor(x=>x.To).NotEmpty().WithMessage("The destination account is required");
        
        RuleFor(x=>x.From).NotEmpty().WithMessage("The source account is required");

        RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount is required");
        
        RuleFor(x=>x.Description).MaximumLength(100).WithMessage("Description length must be less than 100 characters");
    }
}