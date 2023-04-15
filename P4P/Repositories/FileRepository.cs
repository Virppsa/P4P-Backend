using P4P.Repositories.Interfaces;

namespace P4P.Repositories;

public class FileRepository : IFileRepository
{
    public string ReadFile(string path)
    {
        return File.Exists(path)
            ? File.ReadAllText(path)
            : throw new FileNotFoundException("File not found by path: " + path);
    }
}
