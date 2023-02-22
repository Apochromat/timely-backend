using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace timely_backend.Services;

public class EmailSender : IEmailSender {
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration) {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message) {
        using var emailMessage = new MimeMessage();

        var config = _configuration.GetSection("EmailConfiguration");
        
        emailMessage.From.Add(new MailboxAddress(config.GetValue<string>("FromName"),
            config.GetValue<string>("FromAddress")));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) {
            Text = message
        };

        using (var client = new SmtpClient()) {
            client.Timeout = (int)TimeSpan.FromSeconds(5).TotalMilliseconds;
            await client.ConnectAsync(config.GetValue<string>("SmtpHost"), 25, false);
            await client.AuthenticateAsync(config.GetValue<string>("UserName"), config.GetValue<string>("Password"));
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}