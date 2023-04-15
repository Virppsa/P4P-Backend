using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using P4P.Exceptions;
using P4P.Filters;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Options;
using P4P.Repositories;
using P4P.Repositories.Interfaces;
using P4P.Services;
using P4P.Services.EmailService;
using P4P.Services.Interfaces;
using P4P.Wrappers;

namespace UnitTests.Services;

[TestClass]
public class LikeServiceTests
{
    private readonly Mock<ILikeRepository> _likeRepository;
    private readonly Mock<IUserService> _userService;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IPostRepository> _postRepository;
    private readonly Mock<ICommentRepository> _commentRepository;

    public LikeServiceTests()
    {
        _likeRepository = new Mock<ILikeRepository>();
        _userService = new Mock<IUserService>();
        _mapper = new Mock<IMapper>();
        _postRepository = new Mock<IPostRepository>();
        _commentRepository = new Mock<ICommentRepository>();
    }

    [TestMethod]
    public async Task GetAll()
    {
        _likeRepository
            .Setup(x => x.GetPaginated(
                It.IsAny<PaginationFilter>(),
                It.IsAny<Expression<Func<Like, bool>>>(),
                It.IsAny<Func<IQueryable<Like>, IOrderedQueryable<Like>>>(),
                It.IsAny<List<string>>()))
            .Returns(new PaginatedList<List<Like>>());

        var sut = new LikeService(_likeRepository.Object, _userService.Object, _postRepository.Object, _commentRepository.Object, _mapper.Object);
        var response = sut.GetAll(new PaginationFilter());

        Assert.AreEqual(response.GetType(), typeof(PaginatedList<List<Like>>));
    }

    [TestMethod]
    public async Task Create()
    {
        _commentRepository
            .Setup(x => x.GetByIdAsync(1))
            .Returns(Task.FromResult(new Comment()));

        _postRepository
            .Setup(x => x.GetByIdAsync(1))
            .Returns(Task.FromResult(new Post()));

        _userService
            .Setup(x => x.GetCurrent())
            .Returns(Task.FromResult(new User
            {
                Id = 1,
                Email = "test@email.com",
            }));

        _mapper
            .Setup(x => x.Map<Like>(
                It.Is<CreateLikeRequest>(r =>
                    r.CommentId == 1 &&
                    r.PostId == 1)))
            .Returns(new Like
            {
                CommentId = 1,
                PostId = 1,
            });

        _likeRepository
            .Setup(x => x.Exists(It.IsAny<Expression<Func<Like, bool>>>()))
            .Returns(Task.FromResult(false));

        var request = new CreateLikeRequest()
        {
            CommentId = 1,
            PostId = 1,
        };

        _likeRepository
            .Setup(x => x.InsertAsync(
                It.Is<Like>(r =>
                r.CommentId == 1 &&
                r.PostId == 1)))
            .Returns(Task.FromResult(new Like
            {
                CommentId = 1,
                PostId = 1,
            }));

        _mapper
            .Setup(x => x.Map<LikeResponse>(
                It.Is<Like>(r =>
                    r.CommentId == 1 &&
                    r.PostId == 1)))
            .Returns(new LikeResponse
            {
                CommentId = 1,
                PostId = 1,
            });

        var sut = new LikeService(_likeRepository.Object, _userService.Object, _postRepository.Object, _commentRepository.Object, _mapper.Object);
        var response = await sut.Create(request);

        Assert.AreEqual(response.CommentId, request.CommentId);
        Assert.AreEqual(response.PostId, request.PostId);
    }

    [TestMethod]
    [ExpectedException(typeof(HttpException))]
    public async Task Create_No_User()
    {
        _commentRepository
            .Setup(x => x.GetByIdAsync(1))
            .Returns(Task.FromResult(new Comment()));

        _postRepository
            .Setup(x => x.GetByIdAsync(1))
            .Returns(Task.FromResult(new Post()));

        _userService
            .Setup(x => x.GetCurrent())
            .Returns(Task.FromResult<User>(null));

        _mapper
            .Setup(x => x.Map<Like>(
                It.Is<CreateLikeRequest>(r =>
                    r.CommentId == 1 &&
                    r.PostId == 1)))
            .Returns(new Like
            {
                CommentId = 1,
                PostId = 1,
            });

        _likeRepository
            .Setup(x => x.Exists(It.IsAny<Expression<Func<Like, bool>>>()))
            .Returns(Task.FromResult(false));

        var request = new CreateLikeRequest()
        {
            CommentId = 1,
            PostId = 1,
        };

        var sut = new LikeService(_likeRepository.Object, _userService.Object, _postRepository.Object, _commentRepository.Object, _mapper.Object);
        var response = await sut.Create(request);
    }

    [TestMethod]
    [ExpectedException(typeof(HttpException))]
    public async Task Create_Invalid()
    {
        _commentRepository
            .Setup(x => x.GetByIdAsync(1))
            .Returns(Task.FromResult<Comment>(null));

        _postRepository
            .Setup(x => x.GetByIdAsync(1))
            .Returns(Task.FromResult<Post>(null));

        _mapper
            .Setup(x => x.Map<Like>(
                It.Is<CreateLikeRequest>(r =>
                    r.CommentId == 1 &&
                    r.PostId == 1)))
            .Returns(new Like
            {
                CommentId = 1,
                PostId = 1,
            });

        _likeRepository
            .Setup(x => x.Exists(It.IsAny<Expression<Func<Like, bool>>>()))
            .Returns(Task.FromResult(false));

        var request = new CreateLikeRequest()
        {
            CommentId = 1,
            PostId = 1,
        };

        var sut = new LikeService(_likeRepository.Object, _userService.Object, _postRepository.Object, _commentRepository.Object, _mapper.Object);
        var response = await sut.Create(request);
    }
}