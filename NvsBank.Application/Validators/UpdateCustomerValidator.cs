using FluentValidation;
using NvsBank.Application.UseCases.Customer.Commands;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.Validators;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomer.UpdateCustomerCommand>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MinimumLength(3).WithMessage("Full name must have at least 3 characters.")
            .MaximumLength(100).WithMessage("Full name must have at most 100 characters.");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[0-9]{8,15}$")
            .WithMessage("Phone number is not valid.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.");

    }
}