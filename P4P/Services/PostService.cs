using AutoMapper;
using P4P.Exceptions;
using P4P.Filters;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Repositories.Interfaces;
using P4P.Services.Interfaces;
using P4P.Wrappers;

namespace P4P.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserService _userService;
    private readonly ILocationRepository _locationRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IMapper _mapper;

    public PostService(
        IPostRepository postRepository,
        IUserService userService,
        ILocationRepository locationRepository,
        IMapper mapper,
        ILikeRepository likeRepository
    )
    {
        _postRepository = postRepository;
        _userService = userService;
        _locationRepository = locationRepository;
        _likeRepository = likeRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<List<PostResponse>>> GetAll(PostFilter filter)
    {
        var requiredEntities = new List<string>() { "User", "Location", "Likes" };

        var paginatedList = _postRepository.GetPaginated(
            filter,
            post => filter.LocationId == null || post.LocationId == filter.LocationId,
            query => query.OrderByDescending(post => post.Id),
            requiredEntities
        );

        var mappedList = _mapper.Map<PaginatedList<List<PostResponse>>>(paginatedList);

        var user = await _userService.GetCurrent();

        if (mappedList.Items == null)
        {
            return mappedList;
        }

        foreach (var post in mappedList.Items)
        {
            if (user != null)
            {
                post.Like.HasLiked =
                    await _likeRepository.GetFirstAsync(filter: x => x.UserId == user.Id && x.PostId == post.Id) !=
                    null;
            }
            else
            {
                post.Like.HasLiked = false;
            }

            post.Like.LikeCount = _likeRepository.Count(x => x.PostId == post.Id);
        }

        return mappedList;
    }

    public async Task<PostResponse> GetSingle(int postId)
    {
        var post = await _postRepository.GetByIdAsync(postId) ??
                   throw new HttpException(
                       StatusCodes.Status404NotFound,
                       "Rekomendacija nerasta"
                   );

        return _mapper.Map<PostResponse>(post);
    }

    public async Task<PostResponse> Create(CreatePostRequest createPostRequest)
    {
        var user = await _userService.GetCurrent() ?? throw new Exception("UserId cannot be null when creating post");
        var location = await _locationRepository.GetByIdAsync(createPostRequest.LocationId) ?? throw new HttpException(
            StatusCodes.Status400BadRequest,
            "Nurodyta lokacija nerasta"
        );
        var post = _mapper.Map<Post>(createPostRequest);

        post.Location = location;
        post.User = user;
        post = await _postRepository.InsertAsync(post);

        return _mapper.Map<PostResponse>(post);
    }
}