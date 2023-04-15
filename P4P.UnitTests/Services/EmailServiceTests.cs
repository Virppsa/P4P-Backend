using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using P4P.Exceptions;
using P4P.Filters;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Options;
using P4P.Repositories;
using P4P.Repositories.Interfaces;
using P4P.Services;
using P4P.Services.EmailService;
using P4P.Services.Interfaces;
using P4P.Wrappers;

namespace UnitTests.Services;

[TestClass]
public class EmailServiceTests
{
    private readonly IOptions<EmailOptions> _options = Options.Create(new EmailOptions()
    {
        FromAddress = "email@mail.com",
        EnableSsl = true,
        FromName = "name",
        Host = "host",
        LinkDomain = "domain",
        Password = "pass",
        Port = 3000,
    });
    private readonly Mock<ILoggerFactory> _loggerFactory;

    public EmailServiceTests()
    {
        _loggerFactory = new Mock<ILoggerFactory>(MockBehavior.Loose);
    }

    [TestMethod]
    public async Task SendEmail()
    {
        var logger = new Mock<ILogger>(MockBehavior.Loose);

        logger.Setup(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

        _loggerFactory
            .Setup(x => x.CreateLogger("file"))
            .Returns(logger.Object);

        var args = new SendEmailArgs("email@mail.com", "token", "verify_email");
        var sut = new EmailService(_loggerFactory.Object, _options);
        
        sut.SendEmail(null, args);
    }
}