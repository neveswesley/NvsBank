using System.Data;
using FluentValidation;
using NvsBank.Application.UseCases.Account.Commands;

namespace NvsBank.Application.UseCases.Account.Validators;

public class UpdateAccountValidator : AbstractValidator<CreateAccountCommand>
{
    public UpdateAccountValidator()
    {
        RuleFor(x=>x.AccountType)
            .NotEmpty().WithMessage("The account type is required");
        
        RuleFor(x=>x.CustomerId)
            .NotEmpty().WithMessage("The customer id is required");
    }
}