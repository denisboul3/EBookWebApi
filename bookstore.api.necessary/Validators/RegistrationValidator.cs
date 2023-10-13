using bookstore.api.DTO;
using FluentValidation;

namespace bookstore.api.necessary.Validators;

public class RegistrationValidator : AbstractValidator<CreateUserDto>
{
    public RegistrationValidator()
    {
        RuleFor(x => x.Login).NotNull();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).Length(4, 16);
    }
}