using Auth.API.Dto.RequestDtos;
using Auth.API.Dto.RequestDtos.Auth;
using FluentValidation;

namespace Auth.API.Validators.Auth;

public class EmailLoginOrRegisterTicketRequestValidator : AbstractValidator<EmailLoginOrRegisterTicketRequest>
{
    public EmailLoginOrRegisterTicketRequestValidator()
    {
        RuleFor(x => x.Credentials)
           .EmailAddress()
           .WithMessage(ValidationMessages.IncorrectEmail);
    }   
}