using Auth.API.Services.Interfaces;

namespace Auth.API.Services;

public class DigitVerificationCodeGeneratorService : IVerificationCodeGeneratorService
{
    private readonly ILogger<DigitVerificationCodeGeneratorService> _logger;

    public DigitVerificationCodeGeneratorService(ILogger<DigitVerificationCodeGeneratorService> logger)
    {
        _logger = logger;
    }
    
    public string GetVerificationCode(string credentials)
    {
        var code = new Random().Next(1000, 9999);
        _logger.LogInformation("Verification code {Code} for {Credentials}", code, credentials);
        return code.ToString();
    }
}