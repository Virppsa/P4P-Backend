using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using P4P.Options;
using P4P.Services.Interfaces;

namespace P4P.Services.EmailService;

public class EmailService : IEmailService
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly EmailOptions _emailOptions;

    internal const string VerifyEmail = "verify_email";

    public EmailService(ILoggerFactory loggerFactory, IOptions<EmailOptions> emailOptions)
    {
        _loggerFactory = loggerFactory;
        _emailOptions = emailOptions.Value;
    }

    public void SendEmail(UserService sender, SendEmailArgs sendEmailArgs)
    {
        var client = CreateClient();
        var message = new MailMessage(_emailOptions.FromAddress, sendEmailArgs.Email)
        {
            Subject = RenderSubject(sendEmailArgs),
            Body = RenderBody(sendEmailArgs),
            IsBodyHtml = true
        };

        client.SendAsync(message, $"Email ({sendEmailArgs.Type}) to {sendEmailArgs.Email}");
    }

    private SmtpClient CreateClient()
    {
        var client = new SmtpClient
        {
            Host = _emailOptions.Host,
            Port = _emailOptions.Port,
            Credentials = new NetworkCredential(_emailOptions.FromAddress, _emailOptions.Password),
            EnableSsl = _emailOptions.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
        };

        client.SendCompleted += SendCompletedCallback;

        return client;
    }

    private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        var logger = _loggerFactory.CreateLogger("File");

        if (e.Cancelled)
        {
            logger.LogWarning("[{Token}] Cancelled", e.UserState);
        }

        if (e.Error != null)
        {
            logger.LogError("[{Token}] {Error}", e.UserState, e.Error.ToString());
        }
    }

    private string RenderSubject(SendEmailArgs sendEmailArgs)
    {
        return sendEmailArgs.Type switch
        {
            VerifyEmail => "Verify your email | People 4 People",
            _ => "People 4 People"
        };
    }

    private string RenderBody(SendEmailArgs sendEmailArgs)
    {
        return sendEmailArgs.Type switch
        {
            VerifyEmail =>
                $"Hi,<br><br>Please verify your email address <a href='{_emailOptions.LinkDomain}/api/verify/{sendEmailArgs.Token}'>here</a>.<br><br>People 4 People",
            _ => "Hi,<br><br>Welcome!<br><br>People 4 People"
        };
    }
}
