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
public class LocationController : ControllerBase
{
    private readonly ILocationService _service;

    public LocationController(ILocationService service)
    {
        _service = service;
    }

    // GET: api/Location
    [HttpGet]
    public PaginatedList<List<LocationResponse>> GetLocations([FromQuery] LocationFilter filter)
    {
        return _service.GetAll(filter);
    }

    // GET: api/Location/{locationId}
    [HttpGet("{locationId}")]
    public Task<LocationByIdResponse> GetLocation([FromRoute] int locationId, [FromQuery] PaginationFilter filter)
    {
        return _service.GetSingle(locationId, filter);
    }

    // POST: api/Location
    [HttpPost]
    [RequestSizeLimit(1024 * 1024)]
    [Authorize]
    public Task<LocationResponse> CreateLocation([FromForm] CreateLocationRequest createLocationRequest)
    {
        return _service.Create(createLocationRequest);
    }

    // POST: api/Location/{locationId}/Rating
    [HttpPost]
    [Authorize]
    [Route("{locationId}/Rating")]
    public Task<LocationResponse> AddRating(
        [FromRoute] int locationId,
        [FromBody] LocationRatingRequest locationRatingRequest
    )
    {
        return _service.AddRating(locationId, locationRatingRequest);
    }
}
