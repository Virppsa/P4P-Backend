using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P4P.Filters;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Services.Interfaces;
using P4P.Wrappers;

namespace P4P.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LikeController : ControllerBase
{
    private readonly ILikeService _service;

    public LikeController(ILikeService service)
    {
        _service = service;
    }

    // POST: api/Like
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<LikeResponse>> CreateLike([FromBody] CreateLikeRequest request)
    {
        return await _service.Create(request);
    }
}
