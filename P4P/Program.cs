using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using P4P.Data;
using P4P.Extensions;
using P4P.Middleware;
using P4P.Services;
using P4P.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IP4PContext>(options => options.GetRequiredService<P4PContext>());
builder.Services.AddDbContext<P4PContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("P4PContext") ??
                      throw new InvalidOperationException("Connection string 'P4PContext' not found.")));

if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddFile();
}

// Add services to the container
builder.Services.AddHttpContextAccessor();
builder.Services.AddDependencyServices(builder);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddHybridModelBinder();
builder.Services.AddJwtAuthentication(builder);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddAuthorization();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddApiVersioning(options => 
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver")
    );
});

builder.Services.AddVersionedApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }
    );

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(autoFacBuilder =>
    {
        autoFacBuilder.Register(c => new VerificationService())
            .As<IVerificationService>()
            .EnableInterfaceInterceptors()
            .InstancePerDependency();

        autoFacBuilder.RegisterType<LogAttempts>()
        .AsSelf()
        .SingleInstance();
    });

var app = builder.Build();
app.UseFileServer();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(new CultureInfo("en-gb")),
    SupportedCultures = new List<CultureInfo>
    {
        new("en-gb")
    },
    SupportedUICultures = new List<CultureInfo>
    {
        new("en-gb")
    }
});
app.Run();

public partial class Program { }
