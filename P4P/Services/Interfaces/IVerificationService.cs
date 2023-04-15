using P4P.Models;

namespace P4P.Services.Interfaces;

public interface IVerificationService
{
    public void Verify(User user, string password);
}
