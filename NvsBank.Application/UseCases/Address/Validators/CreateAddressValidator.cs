using FluentValidation;
using NvsBank.Application.UseCases.Address.Commands;

namespace NvsBank.Application.UseCases.Address.Validators;

public class CreateAddressValidator : AbstractValidator<CreateAddress.CreateAddressCommand>
{
    public CreateAddressValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("The street is required")
            .MinimumLength(1).WithMessage("The street must be at least 2 characters")
            .MaximumLength(50).WithMessage("The street must be less than 50 characters");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("The number is required")
            .MaximumLength(10).WithMessage("The number must be less than 10 characters");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("The city is required")
            .MinimumLength(2).WithMessage("The city must be at least 2 characters")
            .MaximumLength(50).WithMessage("The city must be less than 50 characters");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("The state is required")
            .Length(2).WithMessage("The state must be at least 2 characters");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("The zip code is required")
            .Length(9).WithMessage("The zip code must be at least 9 characters");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("The customer id is required");
    }
}