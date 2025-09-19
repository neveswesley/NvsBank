using FluentValidation;
using NvsBank.Application.UseCases.Customer.Commands;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Application.Validators;

public class CreateCustomerValidator : AbstractValidator<CreateCustomer.CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
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
            .Matches(@"^\+?[0-9]{8,15}$");
    }
}