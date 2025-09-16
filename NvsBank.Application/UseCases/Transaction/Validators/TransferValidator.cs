using FluentValidation;
using NvsBank.Application.UseCases.PixKey.Commands;

namespace NvsBank.Application.UseCases.Transaction.Validators;

public class TransferValidator : AbstractValidator<PixTransferCommand>
{
    public TransferValidator()
    {
        RuleFor(x=>x.FromAccountId).NotEmpty().WithMessage("From account id is required");
        
        RuleFor(x=>x.ToPixKey).NotEmpty().WithMessage("To account id is required");
        
        RuleFor(x=>x.Amount).NotEmpty().WithMessage("Amount is required");
        
        RuleFor(x=>x.Description).MaximumLength(100).WithMessage("Description length must be less than 100 characters");
    }   
}