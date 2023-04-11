using Auth.API.Dto.RequestDtos.User;
using FluentValidation;

namespace Auth.API.Validators.Me;

public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
{
    public UserUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(6)
            .WithMessage(ValidationMessages.TooShortName)
            .MaximumLength(50)
            .WithMessage(ValidationMessages.TooLongName)
            // не работает с ё
            .Matches(@"^[a-zA-Zа-яА-Я]+(\s[a-zA-Zа-яА-Я]+)*$")
            .WithMessage(ValidationMessages.NameContainsWrongSymbols);
    }
}