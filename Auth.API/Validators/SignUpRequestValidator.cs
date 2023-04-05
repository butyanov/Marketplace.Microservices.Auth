using Auth.API.Dto.RequestDtos;
using FluentValidation;

namespace Auth.API.Validators;

public class SignUpRequestValidator : AbstractValidator<SignupRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmptyName)
            .MinimumLength(6)
            .WithMessage(ValidationMessages.TooShortName)
            .MaximumLength(50)
            .WithMessage(ValidationMessages.TooLongName)
            // не работает с ё
            .Matches(@"^[a-zA-Zа-яА-Я]+(\s[a-zA-Zа-яА-Я]+)*$")
            .WithMessage(ValidationMessages.NameContainsWrongSymbols);
       
     
    }
}