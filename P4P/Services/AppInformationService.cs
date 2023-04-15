using P4P.Constants;
using P4P.Models;
using P4P.Services.Interfaces;

namespace P4P.Services;

public class AppInformationService : IAppInformationService
{
    private readonly IFileService<AppInformation> _fileService;
    
    public AppInformationService(IFileService<AppInformation> fileService)
    {
        _fileService = fileService;
    }
    
    public AppInformation Get()
    {
        return _fileService.ParseJsonFile(AppInformationConstants.AppInformationFilePath);
    }

    public string? GetVersion()
    {
        return Get().Version;
    }
}
