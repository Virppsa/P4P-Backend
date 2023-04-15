using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using P4P.Exceptions;
using P4P.Repositories.Interfaces;
using P4P.Services.Interfaces;

namespace P4P.Services;

public class FileService<T> : IFileService<T>
{
    private readonly IFileRepository _fileRepository;
    private readonly IWebHostEnvironment _environment;

    public FileService(IFileRepository fileRepository, IWebHostEnvironment environment)
    {
        _fileRepository = fileRepository;
        _environment = environment;
    }
    
    public T ParseJsonFile(string path)
    {
        var file = _fileRepository.ReadFile(path);

        return JsonConvert.DeserializeObject<T>(file) ??
               throw new SerializationException("Error occurred while deserializing json file by path:" + path);
    }

    public async Task<string> SaveImageAsync(IFormFile image)
    {
        string uniqueName = Guid.NewGuid().ToString();

        string fileName;
        switch (image.ContentType.Split('/')[1])
        {
            case "jpeg":
                fileName = uniqueName + ".jpeg";
                break;
            case "jpg":
                fileName = uniqueName + ".jpg";
                break;
            case "png":
                fileName = uniqueName + ".png";
                break;
            default:
                throw new HttpException(
                    StatusCodes.Status400BadRequest,
                    "Blogas nuotraukos tipas. Galimi: png, jpg, jpeg"
                );
        }

        var path = Path.Combine(_environment.WebRootPath, typeof(T).ToString() , fileName);

        Directory.CreateDirectory(Path.GetDirectoryName(path));

        await image.CopyToAsync(new FileStream(path, FileMode.Create));

        return (typeof(T).ToString() + '/' + fileName);
    }
}
