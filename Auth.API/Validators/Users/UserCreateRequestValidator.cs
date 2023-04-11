using Auth.API.Dto.RequestDtos.User;
using FluentValidation;

namespace Auth.API.Validators.Users;

public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
{
    public UserCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(6)
            .WithMessage(ValidationMessages.TooShortName)
            .MaximumLength(50)
            .WithMessage(ValidationMessages.TooLongName)
            // не работает с ё
            .Matches(@"^[a-zA-Zа-яА-Я]+(\s[a-zA-Zа-яА-Я]+)*$")
            .WithMessage(ValidationMessages.NameContainsWrongSymbols);
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ValidationMessages.IncorrectEmail);
        RuleFor(x => x.PhoneNumber)
            .MinimumLength(11)
            .WithMessage(ValidationMessages.TooShortPhone)
            .MaximumLength(19)
            .WithMessage(ValidationMessages.TooLongPhone)
            .Matches(@"^(?:\+[\d]{1,3}[\s-])?[\d]{8,15}$")
            .WithMessage(ValidationMessages.IncorrectPhone);
    }
}