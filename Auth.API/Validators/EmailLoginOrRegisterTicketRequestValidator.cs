using Auth.API.Dto.RequestDtos;
using FluentValidation;

namespace Auth.API.Validators;

public class EmailLoginOrRegisterTicketRequestValidator : AbstractValidator<EmailLoginOrRegisterTicketRequest>
{
    public EmailLoginOrRegisterTicketRequestValidator()
    {
        RuleFor(x => x.Credentials)
           .EmailAddress()
           .WithMessage(ValidationMessages.IncorrectEmail);
    }   
}