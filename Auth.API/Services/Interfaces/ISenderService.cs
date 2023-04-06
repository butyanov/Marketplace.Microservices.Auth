namespace Auth.API.Services.Interfaces;

public interface ISenderService
{
    public Task SendAsync(string email, string code, string? subject = "Verification Code");
}