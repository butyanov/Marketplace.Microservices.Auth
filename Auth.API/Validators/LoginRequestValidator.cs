using Auth.API.Dto.RequestDtos;
using FluentValidation;

namespace Auth.API.Validators;

public class LoginRequestValidator : AbstractValidator<AuthorizationRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ValidationMessages.IncorrectEmail);
    }
}