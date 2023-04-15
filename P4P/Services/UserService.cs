using Microsoft.AspNetCore.Razor.TagHelpers;
using AutoMapper;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Repositories.Interfaces;
using P4P.Services.Interfaces;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using P4P.Exceptions;
using P4P.Services.EmailService;

namespace P4P.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private event EmailEventHandler<SendEmailArgs> _userCreated;
    private readonly IFileService<User> _fileService;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        IAuthenticationService authenticationService,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor, 
        IFileService<User> fileService
    )
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _authenticationService = authenticationService;
        _userCreated += emailService.SendEmail;
        _httpContextAccessor = httpContextAccessor;
        _fileService = fileService;
    }

    public async Task<UserResponse> Create(RegisterRequest registerRequest)
    {


        var user = _mapper.Map<User>(registerRequest);

        if (registerRequest.Image == null)
        {
            user.ImageName = "P4P.Models.User/defaultUser.png";
        }
        else
        {
            user.ImageName = await _fileService.SaveImageAsync(registerRequest.Image);
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        user = await _userRepository.InsertAsync(user);

        // 5. Events and their usage: standard and custom;
        _userCreated.Invoke(
            this,
            new SendEmailArgs(user.Email, _authenticationService.GenerateToken(user), EmailService.EmailService.VerifyEmail)
        );

        return _mapper.Map<UserResponse>(user);
    }

    public async Task<User?> GetSingle(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<bool> ExistsByEmail(string email)
    {
        return await _userRepository.ExistsByEmail(email);
    }

    public async Task<User?> GetCurrent()
    {
        var identity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
        var userIdString = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int userId;

        if (userIdString == null)
        {
            return null;
        }

        try
        {
            userId = int.Parse(userIdString);
        }
        catch (FormatException)
        {
            return null;
        }

        return await _userRepository.GetByIdAsync(userId);
    }
}
