namespace Auth.API.Services.Interfaces;

public interface IVerificationCodeGeneratorService
{
    string GetVerificationCode(string credentials);
}