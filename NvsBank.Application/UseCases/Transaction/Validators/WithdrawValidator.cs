using FluentValidation;
using NvsBank.Application.UseCases.Transaction.Commands;

namespace NvsBank.Application.UseCases.Transaction.Validators;

public class WithdrawValidator : AbstractValidator<Withdraw.WithdrawCommand>
{
    public WithdrawValidator()
    {
        RuleFor(x=>x.Id).NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount is required");
        
        RuleFor(x=>x.Description).MaximumLength(100).WithMessage("Description length must be less than 100 characters");
    }
}