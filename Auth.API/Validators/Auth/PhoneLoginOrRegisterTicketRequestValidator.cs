using Auth.API.Dto.RequestDtos.Auth;
using FluentValidation;

namespace Auth.API.Validators.Auth;

public class PhoneLoginOrRegisterTicketRequestValidator : AbstractValidator<PhoneLoginOrRegisterTicketRequest>
{
    public PhoneLoginOrRegisterTicketRequestValidator()
    {
        RuleFor(x => x.Credentials)
            .MinimumLength(11)
            .WithMessage(AuthValidationMessages.TooShortPhone)
            .MaximumLength(19)
            .WithMessage(AuthValidationMessages.TooLongPhone)
            .Matches(@"^(?:\+[\d]{1,3}[\s-])?[\d]{8,15}$")
            .WithMessage(AuthValidationMessages.IncorrectPhone);
    }
}