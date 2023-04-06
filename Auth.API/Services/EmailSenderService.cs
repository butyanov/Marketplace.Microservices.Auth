using Auth.API.Exceptions;
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
        var userName = _configuration.GetValue<string>("MailCredentials:Username");
        var password = _configuration.GetValue<string>("MailCredentials:Password");
        var host = _configuration.GetValue<string>("Mailing:Host");
        var port = _configuration.GetValue<int>("Mailing:Port");

        using var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Trendsklad MarketPlace", userName));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
        <html>
            <head>
                <meta charset=""utf-8"">
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        font-size: 16px;
                        line-height: 1.6;
                        color: #333;
                        background-color: #f7f7f7;
                    }}
                    h1,h3 {{
                        font-size: 28px;
                        font-weight: bold;
                        margin-top: 0;
                    }}
                </style>
            </head>
            <body>
                <h1>{subject}</h1>
                <h3>{message}</p>
            </body>
        </html>"
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        {
            try
            {
                await client.ConnectAsync(host, port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(userName, password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception)
            {
                throw new BadRequestException("Incorrect credentials were entered");
            }
        }
    }
}