using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Moq;
using P4P.Models;
using P4P.Repositories.Interfaces;
using P4P.Services;

namespace UnitTests.Services;

[TestClass]
public class FileServiceTest
{
    private readonly Mock<IFileRepository> _fileRepository;
    private readonly Mock<IWebHostEnvironment> _environment;

    public FileServiceTest()
    {
        _fileRepository = new Mock<IFileRepository>();
        _environment = new Mock<IWebHostEnvironment>();
    }

    [TestMethod]
    public async Task ParseJsonFile()
    {
        var post = new Post
        {
            Id = 1,
            Text = "text",
        };

        _fileRepository
            .Setup(x => x.ReadFile("path"))
            .Returns(JsonSerializer.Serialize(post));

        var sut = new FileService<Post>(_fileRepository.Object, _environment.Object);
        var response = sut.ParseJsonFile("path");

        Assert.AreEqual(response.Id, 1);
        Assert.AreEqual(response.Text, "text");

        _fileRepository.VerifyAll();
    }
}