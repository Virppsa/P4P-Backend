namespace P4P.Services.Interfaces;

public interface IFileService<T>
{
    public T ParseJsonFile(string path);

    public Task<string> SaveImageAsync(IFormFile image);
}
