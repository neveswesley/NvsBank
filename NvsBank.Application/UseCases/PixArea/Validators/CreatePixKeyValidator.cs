using FluentValidation;
using NvsBank.Application.UseCases.PixKey.Commands;

namespace NvsBank.Application.UseCases.PixKey.Validators;

public class CreatePixKeyValidator : AbstractValidator<CreatePixKey.CreatePixKeyCommand>
{
    public CreatePixKeyValidator()
    {
        RuleFor(x=>x.AccountId)
            .NotEmpty().WithMessage("Account Id is required");
        
        RuleFor(x=>x.KeyType)
            .NotEmpty().WithMessage("Key type is required");
        
        RuleFor(x=>x.KeyValue)
            .NotEmpty().WithMessage("KeyValue is required");
        
    }
}