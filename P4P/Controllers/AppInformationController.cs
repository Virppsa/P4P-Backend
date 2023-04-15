using Microsoft.AspNetCore.Mvc;
using P4P.Models;
using P4P.Services.Interfaces;

namespace P4P.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppInformationController : ControllerBase
{
    private readonly IAppInformationService _service;

    public AppInformationController(IAppInformationService service)
    {
        _service = service;
    }

    // GET: api/AppInformation
    [HttpGet]
    public AppInformation Get()
    {
        return _service.Get();
    }
}
