using chuyennganh.Application.Repositories;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using chuyennganh.Infrasture.Context;

public class EmailService : IEmailService
{
    private readonly EmailSettings emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettingsOptions)
    {
        emailSettings = emailSettingsOptions.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(emailSettings.SenderName, emailSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = body };
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(emailSettings.SmtpServer, emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(emailSettings.SenderEmail, emailSettings.SenderPassword);

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
