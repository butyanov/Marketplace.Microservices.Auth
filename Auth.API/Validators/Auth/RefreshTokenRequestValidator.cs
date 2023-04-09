﻿using Auth.API.Dto.RequestDtos.Auth;
using FluentValidation;

namespace Auth.API.Validators.Auth;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage(AuthValidationMessages.EmptyBearerToken);
        
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage(AuthValidationMessages.EmptyRefreshToken);
    }
}