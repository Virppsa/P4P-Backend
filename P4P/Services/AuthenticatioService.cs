using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using P4P.Exceptions;
using P4P.Middleware;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Options;
using P4P.Repositories.Interfaces;
using P4P.Services.Interfaces;

namespace P4P.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRefreshTokenRepository _authenticationRepository;
    private readonly IUserRepository _userRepository;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtOptions _jwtOptions;
    private readonly IVerificationService _verificationService;

    public AuthenticationService(
        IUserRepository userRepository,
        IUserRefreshTokenRepository authenticationRepository,
        JwtSecurityTokenHandler tokenHandler,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtOptions> jwtOptions,
        IVerificationService verificationService
    )
    {
        _userRepository = userRepository;
        _authenticationRepository = authenticationRepository;
        _tokenHandler = tokenHandler;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _jwtOptions = jwtOptions.Value;
        _verificationService = verificationService;
    }

    public virtual async Task<LoginResponse> Login(LoginRequest loginRequest)
    {
        var user = _mapper.Map<User>(loginRequest);

        try
        {
            user = await AuthorizeUser(user);
        }
        catch (AuthenticationException)
        {
            throw new HttpException(
                StatusCodes.Status401Unauthorized,
                "El. paštas ar slaptažodis netinka"
            );
        }

        if (!user.Verified)
        {
            throw new HttpException(
                StatusCodes.Status401Unauthorized,
                "Patvirtinkite el. paštą ir bandykite dar kartą"
            );
        }

        var token = GenerateToken(user);
        var refreshToken = await CreateRefreshToken(user);
        var userResponse = _mapper.Map<UserResponse>(user);

        return new LoginResponse
            { Message = "Sėkmingai prisijungėte", Data = userResponse, Token = token, RefreshToken = refreshToken };
    }

    public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var userRefreshToken = await AuthorizeRefreshToken(refreshTokenRequest);
        var token = GenerateToken(userRefreshToken.User);
        var refreshToken = GenerateRefreshToken();

        userRefreshToken.ExpirationDate = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshTokenValidityInMinutes);
        userRefreshToken.Token = refreshToken;

        await _authenticationRepository.UpdateAsync(userRefreshToken);

        return new RefreshTokenResponse
            { Token = token, RefreshToken = refreshToken };
    }

    public async Task<ActionResult> Logout(LogoutRequest logoutRequest)
    {
        var userRefreshToken =
            await _authenticationRepository.GetFirstAsync(x => x.Token == logoutRequest.RefreshToken);

        if (userRefreshToken != null)
        {
            await _authenticationRepository.DeleteAsync(userRefreshToken.Id);
        }

        return new OkResult();
    }

    private async Task<string> CreateRefreshToken(User user)
    {
        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ??
                        throw new Exception("IP address could not be determined when creating refresh token");
        var refreshToken = GenerateRefreshToken();
        var userRefreshToken = new UserRefreshToken
        {
            CreatedDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshTokenValidityInMinutes),
            IpAddress = ipAddress,
            Token = refreshToken,
            UserId = user.Id
        };

        await _authenticationRepository.InsertAsync(userRefreshToken);

        return refreshToken;
    }

    public async Task<GenericResponse> Verify(string token)
    {
        JwtSecurityToken? decodedToken;

        try
        {
            decodedToken = _tokenHandler.ReadToken(token) as JwtSecurityToken;
        }
        catch (ArgumentException)
        {
            throw new InvalidEmailVerificationTokenException();
        }

        var id = decodedToken?.Claims.First(c => c.Type == JwtRegisteredClaimNames.NameId).Value;

        if (id == null)
        {
            throw new InvalidEmailVerificationTokenException();
        }

        var user = await _userRepository.GetByIdAsync(int.Parse(id));

        if (user == null)
        {
            throw new InvalidEmailVerificationTokenException();
        }

        user.Verified = true;
        await _userRepository.UpdateAsync(user);

        return new GenericResponse { Message = "Email was verified successfully" };
    }

    public string GenerateToken(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Image", user.ImageName)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.JwtValidityInMinutes),
            Audience = _jwtOptions.Audience,
            Issuer = _jwtOptions.Issuer,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
                SecurityAlgorithms.HmacSha256
            )
        };
        var token = _tokenHandler.CreateToken(tokenDescriptor);
        var serializedToken = _tokenHandler.WriteToken(token);

        return serializedToken;
    }

    private static string GenerateRefreshToken()
    {
        var byteArray = new byte[64];

        RandomNumberGenerator.Create().GetBytes(byteArray);

        return Convert.ToBase64String(byteArray);
    }

    private async Task<User> AuthorizeUser(User userInput)
    {
        var user = await _userRepository.GetFirstAsync(x => x.Email == userInput.Email);

        if (user == null)
        {
            throw new AuthenticationException($"User was not found");
        }

        _verificationService.Verify(user, userInput.Password);

        return user;
    }

    private async Task<UserRefreshToken> AuthorizeRefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var userRefreshToken = await _authenticationRepository.GetFirstAsync(
            x => x.Token == refreshTokenRequest.RefreshToken,
            new List<string> { "User" }
        );
        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ??
                        throw new Exception("IP address could not be determined when authorizing refresh token");

        if (userRefreshToken == null || ipAddress != userRefreshToken.IpAddress)
        {
            throw new HttpException(
                StatusCodes.Status400BadRequest,
                "Įvyko klaida, bandykite prisijungti iš naujo"
            );
        }

        if (userRefreshToken.ExpirationDate < DateTime.UtcNow)
        {
            throw new HttpException(
                StatusCodes.Status400BadRequest,
                "Įvyko klaida, bandykite prisijungti iš naujo"
            );
        }

        return userRefreshToken;
    }
}