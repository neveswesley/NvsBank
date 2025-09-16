using FluentValidation;
using NvsBank.Application.UseCases.Customer.Commands;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.Validators;

public class UpdateCustomerStatusValidator : AbstractValidator<UpdateCustomerStatus.ChangeCustomerStatusCommand>
{
    public UpdateCustomerStatusValidator()
    {
        RuleFor(x => x.Status)
            .NotNull().WithMessage("The status field is required");
        
        RuleFor(x => x.Reason)
            .NotNull().WithMessage("The reason field is required")
            .MaximumLength(50).WithMessage("The reason field must not exceed 50 characters");
    }
}