using FluentValidation;
using NvsBank.Application.UseCases.BankSlip.Command;

namespace NvsBank.Application.UseCases.BankSlip.Validators;

public class CreateBankSlipValidator : AbstractValidator<CreateBankSlip.CreateBankSlipCommand>
{
    public CreateBankSlipValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("The amount is required.");

        RuleFor(x => x.AccountPayeeId)
            .NotEmpty().WithMessage("The account payee id is required.");
        
        RuleFor(x => x.CustomerPayerId)
            .NotEmpty().WithMessage("The customer payer id is required.");
    }    
}