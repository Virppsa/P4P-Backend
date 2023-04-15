using P4P.Models;

namespace P4P.Services.Interfaces;

public interface IAppInformationService
{
    public AppInformation Get();
    
    public string? GetVersion();
}
