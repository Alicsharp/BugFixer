using BugFixer.Application.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;




namespace BugFixer.infrastructure.EmailSender
{
 
   
    public class EmailService : IEmailSend
    {
       private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public void SendEmail(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config["EmailSettings:Email"]));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        smtp.Connect(_config["EmailSettings:Host"], int.Parse(_config["EmailSettings:Port"]), SecureSocketOptions.StartTls);
        smtp.Authenticate(_config["EmailSettings:Email"], _config["EmailSettings:Password"]);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
    }
}
