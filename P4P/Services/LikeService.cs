using AutoMapper;
using P4P.Exceptions;
using P4P.Filters;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Repositories;
using P4P.Repositories.Interfaces;
using P4P.Services.Interfaces;
using P4P.Wrappers;

namespace P4P.Services;

public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;

    public LikeService(
        ILikeRepository likeRepository,
        IUserService userService,
        IPostRepository postRepository,
        ICommentRepository commentRepository,
        IMapper mapper
    )
    {
        _likeRepository = likeRepository;
        _userService = userService;
        _mapper = mapper;
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public PaginatedList<List<Like>> GetAll(PaginationFilter filter)
    {
        return _likeRepository.GetPaginated(filter);
    }

    public async Task<LikeResponse> Create(CreateLikeRequest createLikeRequest)
    {
        var like = _mapper.Map<Like>(createLikeRequest);

        if (!await ValidRequest(like))
        {
            throw new HttpException(
                StatusCodes.Status400BadRequest,
                "Įvyko klaida, bandykite dar kartą"
                );
        }

        var user = await _userService.GetCurrent();

        if (user == null)
        {
            throw new HttpException(
                StatusCodes.Status422UnprocessableEntity,
                "Įvyko klaida, bandykite dar kartą"
            );
        }

        if (await _likeRepository.Exists(x =>
                (x.PostId == (createLikeRequest.PostId ?? 0)|| x.CommentId == (createLikeRequest.CommentId ?? 0)) &&
                x.UserId == user.Id))
        {
            throw new HttpException(
                StatusCodes.Status400BadRequest,
                "Šią rekomedaciją jau patikote"
            );
        }

        like.User = user;

        return _mapper.Map<LikeResponse>(await _likeRepository.InsertAsync(like));
    }

    private async Task<bool> ValidRequest(Like like)
    {
        bool validRequest = false;

        if (like.CommentId.HasValue)
        {
            if (await _commentRepository.GetByIdAsync(like.CommentId.Value) != null)
            {
                validRequest = true;
            }
        }

        if (like.PostId.HasValue)
        {
            if (await _postRepository.GetByIdAsync(like.PostId.Value) != null)
            {
                validRequest = true;
            }
        }

        return validRequest;
    }
}
