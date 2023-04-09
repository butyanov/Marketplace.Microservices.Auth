using Auth.API.Dto.RequestDtos.Auth;
using FluentValidation;

namespace Auth.API.Validators.Auth;

public class SignUpRequestValidator : AbstractValidator<SignupRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(AuthValidationMessages.EmptyName)
            .MinimumLength(6)
            .WithMessage(AuthValidationMessages.TooShortName)
            .MaximumLength(50)
            .WithMessage(AuthValidationMessages.TooLongName)
            // не работает с ё
            .Matches(@"^[a-zA-Zа-яА-Я]+(\s[a-zA-Zа-яА-Я]+)*$")
            .WithMessage(AuthValidationMessages.NameContainsWrongSymbols);
       
     
    }
}