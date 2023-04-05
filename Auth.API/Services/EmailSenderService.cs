using Auth.API.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Auth.API.Services;

public class EmailSenderService : ISenderService
{
    private readonly IConfiguration _configuration;

    public EmailSenderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(string email, string message, string? subject = "Verification Code")
    {
        var userName = _configuration.GetValue<string>("MailCredentials:Username")!;
        var password = _configuration.GetValue<string>("MailCredentials:Password")!;
        var host = "smtp.mail.ru";
        var port = 465;
        
        using var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Trendsklad MarketPlace", userName));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = "Верификация почты";
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using var client = new SmtpClient();
        {
            await client.ConnectAsync(host, port, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(userName, password);
            await client.SendAsync(emailMessage);
 
            await client.DisconnectAsync(true);
        }
    }
}