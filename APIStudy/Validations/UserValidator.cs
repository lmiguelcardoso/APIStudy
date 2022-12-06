using APIStudy.Models;
using FluentValidation;

namespace APIStudy.Validations
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user =>user.Name).NotEmpty().NotNull().MaximumLength(30).MinimumLength(10);
            RuleFor(user=>user.Role).NotEmpty().NotNull();
            RuleFor(user=>user.Telephone).NotEmpty().NotNull().ToString();
        }
    }
}
