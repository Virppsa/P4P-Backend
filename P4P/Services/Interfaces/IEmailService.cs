using P4P.Services.EmailService;

namespace P4P.Services.Interfaces;

public interface IEmailService
{
    void SendEmail(UserService sender, SendEmailArgs args);
}
