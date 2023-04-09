using Auth.API.Dto.RequestDtos.Auth;
using FluentValidation;

namespace Auth.API.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<AuthorizationRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(AuthValidationMessages.IncorrectEmail);
    }
}