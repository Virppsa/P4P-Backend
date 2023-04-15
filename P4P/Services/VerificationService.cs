using Autofac.Extras.DynamicProxy;
using P4P.Exceptions;
using P4P.Middleware;
using P4P.Models;
using P4P.Services.Interfaces;

namespace P4P.Services;

[Intercept(typeof(LogAttempts))]
public class VerificationService: IVerificationService
{
    public virtual void Verify(User user, string password)
    {
        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            throw new AuthenticationException($"Incorrect password provided", user.Email);
        }
    }
}
