using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using PassDefender.Host.Configs;

namespace PassDefender.Host.Services.Email;

public class EmailService(IOptions<SmtpConfig> smtpOptions) : IEmailService
{
    private readonly SmtpConfig _smtpConfig = smtpOptions.Value;
    
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(_smtpConfig.Name, _smtpConfig.Username));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, _smtpConfig.UseSsl);
        await client.AuthenticateAsync(_smtpConfig.Username, _smtpConfig.Password);
        await client.SendAsync(emailMessage);

        await client.DisconnectAsync(true);
    }
}