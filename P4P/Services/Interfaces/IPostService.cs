using P4P.Filters;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Wrappers;

namespace P4P.Services.Interfaces;

public interface IPostService
{
    Task<PaginatedList<List<PostResponse>>> GetAll(PostFilter filter);

    Task<PostResponse> GetSingle(int postId);

    Task<PostResponse> Create(CreatePostRequest createPostRequest);
}
