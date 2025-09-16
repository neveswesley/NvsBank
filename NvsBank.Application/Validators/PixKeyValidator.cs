using FluentValidation;
using NvsBank.Domain.Entities.Enums;

public class PixKeyRequest
{
    public Guid AccountId { get; set; }
    public PixKeyType Type { get; set; } // Email, CPF, CNPJ, Phone, EVP
    public string Value { get; set; }
}

public class PixKeyValidator : AbstractValidator<PixKeyRequest>
{
    public PixKeyValidator()
    {
        RuleFor(x => x.Type).NotEmpty();

        RuleFor(x => x.Value)
            .NotEmpty()
            .Must((model, value) => ValidateKey(model.Type.ToString(), value))
            .WithMessage("Valor inválido para o tipo de chave informado.");
    }

    private bool ValidateKey(string type, string value)
    {
        return type switch
        {
            "Email" => IsValidEmail(value),
            "CPF" => IsValidCpf(value),
            "CNPJ" => IsValidCnpj(value),
            "Phone" => IsValidPhone(value),
            "EVP" => true,
            _ => false
        };
    }

    private bool IsValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email) && 
        new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email);

    private bool IsValidCpf(string cpf)
    {
        // aqui você pode implementar a lógica real de CPF (11 dígitos + dígitos verificadores)
        return cpf != null && cpf.All(char.IsDigit) && cpf.Length == 11;
    }

    private bool IsValidCnpj(string cnpj)
    {
        // validação simples, só números e 14 dígitos
        return cnpj != null && cnpj.All(char.IsDigit) && cnpj.Length == 14;
    }

    private bool IsValidPhone(string phone)
    {
        // Aceitando só formato brasileiro simples (ex: 5511999999999)
        return phone != null && phone.All(char.IsDigit) && phone.Length >= 10 && phone.Length <= 13;
    }
}