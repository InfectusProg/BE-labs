using BE_lab2.Models;
using FluentValidation;

namespace BE_lab2.Validators;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(category => category.CategoryName).NotEmpty().MaximumLength(30);
    }
}
