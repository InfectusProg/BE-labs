using BE_lab2.Models;
using FluentValidation;

namespace BE_lab2.Validators;

public class CurrencyValidator : AbstractValidator<Currency>
{
    public CurrencyValidator()
    {
        RuleFor(currency => currency.Name).NotEmpty().MaximumLength(30);
    }
}
