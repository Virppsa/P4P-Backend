using AutoMapper;
using P4P.Repositories.Interfaces;
using P4P.Services.Interfaces;

namespace P4P.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public CommentService(
        ICommentRepository commentRepository,
        IMapper mapper
    )
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }
}
