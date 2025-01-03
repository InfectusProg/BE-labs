using BE_lab2.Models;
using FluentValidation;

namespace BE_lab2.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().MaximumLength(10);
    }
}
