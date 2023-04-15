using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Repositories.Interfaces;
using P4P.Services;
using P4P.Services.EmailService;
using P4P.Services.Interfaces;

namespace UnitTests.Services;

[TestClass]
public class UserServiceTest
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IAuthenticationService> _authenticationService;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly Mock<IEmailService> _emailService;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IFileService<User>> _fileService;

    public UserServiceTest()
    {
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _authenticationService = new Mock<IAuthenticationService>(MockBehavior.Strict);
        _emailService = new Mock<IEmailService>(MockBehavior.Strict);
        _httpContextAccessor = new Mock<IHttpContextAccessor>(MockBehavior.Strict);
        _mapper = new Mock<IMapper>(MockBehavior.Strict);
        _fileService = new Mock<IFileService<User>>(MockBehavior.Strict);
    }

    [TestMethod]
    public async Task Create()
    {
        var request = new RegisterRequest()
        {
            Email = "test@email.com",
            Name = "name",
            Password = "pass",
        };

        _userRepository
            .Setup(x => x.InsertAsync(
                It.Is<User>(u =>
                    u.Email == "test@email.com" &&
                    u.Name == "name")))
            .Returns(Task.FromResult(new User
            {
                Id = 1,
                Email = "test@email.com",
                Name = "name",
                Password = "pass",
                Verified = false,
            }));

        _authenticationService
            .Setup(x => x.GenerateToken(
                It.Is<User>(u =>
                    u.Email == "test@email.com" &&
                    u.Name == "name")))
            .Returns("token");

        _emailService
            .Setup(x => x.SendEmail(
                It.IsAny<UserService>(),
                It.Is<SendEmailArgs>(a =>
                    a.Email == "test@email.com" &&
                    a.Token == "token")));

        _mapper
            .Setup(x => x.Map<User>(
                It.Is<RegisterRequest>(r =>
                    r.Email == "test@email.com" &&
                    r.Password == "pass" &&
                    r.Name == "name")))
            .Returns(new User
            {
                Email = "test@email.com",
                Name = "name",
                Password = "pass",
            });

        _mapper
            .Setup(x => x.Map<UserResponse>(
                It.Is<User>(r =>
                    r.Email == "test@email.com" &&
                    r.Password == "pass" &&
                    r.Name == "name")))
            .Returns(new UserResponse
            {
                Email = "test@email.com",
                Name = "name",
            });

        var sut = new UserService(_userRepository.Object, _mapper.Object, _authenticationService.Object, _emailService.Object, null, _fileService.Object);
        var response = await sut.Create(request);

        Assert.AreEqual(response.Name, "name");
        Assert.AreEqual(response.Email, "test@email.com");

        _userRepository.VerifyAll();
        _authenticationService.VerifyAll();
        _emailService.VerifyAll();
        _mapper.VerifyAll();
    }

    [TestMethod]
    public async Task GetSingle()
    {
        _userRepository
            .Setup(x => x.GetByIdAsync(1))
            .Returns(Task.FromResult(new User
            {
                Id = 1,
                Email = "test@email.com",
                Name = "name",
                Password = "pass",
                Verified = false,
            }));

        var sut = new UserService(_userRepository.Object, null, null, _emailService.Object, null, _fileService.Object);
        var response = await sut.GetSingle(1);

        Assert.AreEqual(response.Name, "name");
        Assert.AreEqual(response.Email, "test@email.com");

        _userRepository.VerifyAll();
    }
}