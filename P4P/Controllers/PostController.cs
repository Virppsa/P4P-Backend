using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P4P.Filters;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Services.Interfaces;
using P4P.Wrappers;

namespace P4P.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _service;

    public PostController(IPostService service)
    {
        _service = service;
    }

    // GET: api/Post
    [HttpGet]
    public async Task<ActionResult<PaginatedList<List<PostResponse>>>> GetPosts([FromQuery] PostFilter filter)
    {
        return await _service.GetAll(filter);
    }
    
    // GET: api/Post/{postId}
    [HttpGet("{postId}")]
    public Task<PostResponse> GetPost([FromRoute] int postId)
    {
        return _service.GetSingle(postId);
    }

    // POST: api/Post
    [HttpPost]
    [Authorize]
    public Task<PostResponse> CreatePosts([FromBody] CreatePostRequest createPostRequest)
    {
        return _service.Create(createPostRequest);
    }
}
