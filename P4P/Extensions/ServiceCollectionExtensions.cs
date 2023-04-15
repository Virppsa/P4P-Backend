using Microsoft.OpenApi.Models;
using P4P.Repositories;
using P4P.Repositories.Interfaces;
using P4P.Services;
using P4P.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using P4P.Options;
using P4P.Services.EmailService;

namespace P4P.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDependencyServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // Options
        services.Configure<EmailOptions>(builder?.Configuration.GetSection(EmailOptions.ObjectKey));
        services.Configure<JwtOptions>(builder?.Configuration.GetSection(JwtOptions.ObjectKey));

        // Repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();

        // Services
        services.AddScoped(typeof(IFileService<>), typeof(FileService<>));
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IAppInformationService, AppInformationService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<JwtSecurityTokenHandler>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICommentService, CommentService>();
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.EnableAnnotations();
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "P4P",
                Description = "People 4 People"
            });

            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Bearer {token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    // 7. Lambda expressions usage;
    public static void AddJwtAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                };
            });
    }
}
