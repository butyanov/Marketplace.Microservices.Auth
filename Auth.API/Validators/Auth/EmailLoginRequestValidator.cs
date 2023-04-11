using Auth.API.Dto.RequestDtos.Auth;
using FluentValidation;

namespace Auth.API.Validators.Auth;

public class EmailLoginRequestValidator : AbstractValidator<EmailAuthorizationRequest>
{
    public EmailLoginRequestValidator()
    {
        RuleFor(x => x.Login)
            .EmailAddress()
            .WithMessage(ValidationMessages.IncorrectEmail);
    }
}