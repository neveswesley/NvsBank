using FluentValidation;
using NvsBank.Application.UseCases.Account.Commands;

namespace NvsBank.Application.UseCases.Account.Validators;

public class UpdateAccountStatusValidator : AbstractValidator<UpdateAccountStatus.UpdateAccountStatusCommand>
{
    public UpdateAccountStatusValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("The account status is required");

        RuleFor(x => x.Reason).NotEmpty()
            .WithMessage("The reason is required");
    }
}