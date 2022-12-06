using APIStudy.Models;
using FluentValidation;

namespace APIStudy.Validations
{
    public class PetValidator : AbstractValidator<Pet>
    {
        public PetValidator()
        {
            RuleFor(pet=>pet.IdOwner).NotEmpty();
            RuleFor(pet=>pet.Name).NotEmpty().MaximumLength(30);
            RuleFor(pet=>pet.Animal).NotEmpty().MaximumLength(20);
            RuleFor(pet=>pet.Race).NotEmpty().MaximumLength(30);
        }
    }
}
