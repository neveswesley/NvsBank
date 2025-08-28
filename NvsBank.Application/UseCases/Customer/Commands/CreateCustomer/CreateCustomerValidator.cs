using FluentValidation;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.UseCases.Employee.Commands.CreateEmployee;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MinimumLength(3).WithMessage("Full name must have at least 3 characters.")
            .MaximumLength(100).WithMessage("Full name must have at most 100 characters.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid customer type.");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("Document number is required.")
            .Length(11, 18).WithMessage("Document number must be between 11 and 18 characters.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().When(x => x.Type == CustomerType.Individual)
            .LessThan(DateTime.Today).When(x => x.BirthDate.HasValue)
            .WithMessage("Birth date must be in the past.");

        RuleFor(x => x.FoundationDate)
            .NotEmpty().When(x => x.Type == CustomerType.Corporate)
            .LessThan(DateTime.Today).When(x => x.FoundationDate.HasValue)
            .WithMessage("Foundation date must be in the past.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[0-9]{8,15}$")
            .WithMessage("Phone number is not valid.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.");

        RuleFor(x => x.CustomerStatus)
            .IsInEnum().WithMessage("Invalid customer status.");
    }
}