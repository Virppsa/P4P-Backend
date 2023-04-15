using System.Linq.Expressions;
using AutoMapper;
using Moq;
using P4P.Filters;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Repositories.Interfaces;
using P4P.Services;
using P4P.Services.Interfaces;
using P4P.Wrappers;

namespace UnitTests.Services;

[TestClass]
public class PostServiceTests
{
    private readonly Mock<IPostRepository> _postRepository;
    private readonly Mock<IUserService> _userService;
    private readonly Mock<ILocationRepository> _locationRepository;
    private readonly Mock<IMapper> _mapper;

    public PostServiceTests()
    {
        _locationRepository = new Mock<ILocationRepository>();
        _mapper = new Mock<IMapper>();
        _postRepository = new Mock<IPostRepository>();
        _userService = new Mock<IUserService>();
    }

    [TestMethod]
    public async Task GetSingle()
    {
        _postRepository
            .Setup(x => x.GetByIdAsync(1))
            .Returns(Task.FromResult(new Post
            {
                Id = 1,
                LocationId = 1,
                UserId = 1
            }));

        _mapper
            .Setup(x => x.Map<PostResponse>(
                It.Is<Post>(r =>
                    r.Id == 1 &&
                    r.LocationId == 1 &&
                    r.UserId == 1)))
            .Returns(new PostResponse
            {
                LocationId = 1,
            });

        var sut = new PostService(_postRepository.Object, null, null, _mapper.Object, null);
        var response = await sut.GetSingle(1);
        Assert.AreEqual(response.LocationId, 1);

        _postRepository.VerifyAll();
        _mapper.VerifyAll();
    }

    [TestMethod]
    public async Task Create()
    {
        var request = new CreatePostRequest()
        {
            LocationId = 1,
            Text = "text",
        };

        _userService
            .Setup(x => x.GetCurrent())
            .Returns(Task.FromResult(new User
            {
                Id = 1,
                Email = "test@email.com",
            }));

        _locationRepository
            .Setup(x => x.GetByIdAsync(1))
            .Returns(Task.FromResult(new Location
            {
                Id = 1,
                Name = "location",
            }));

        _mapper
            .Setup(x => x.Map<Post>(
                It.Is<CreatePostRequest>(r =>
                    r.LocationId == 1 &&
                    r.Text == "text")))
            .Returns(new Post
            {
                LocationId = 1,
                Text = "text",
            });

        _postRepository
            .Setup(x => x.InsertAsync(
                It.Is<Post>(r =>
                r.Text == "text" &&
                r.LocationId == 1)))
            .Returns(Task.FromResult(new Post
            {
                Id = 1,
                Text = "text",
                LocationId = 1,
            }));

        _mapper
            .Setup(x => x.Map<PostResponse>(
                It.Is<Post>(r =>
                    r.Id == 1 &&
                    r.LocationId == 1 &&
                    r.Text == "text")))
            .Returns(new PostResponse
            {
                Id = 1,
                LocationId = 1,
                Text = "text",
            });

        var sut = new PostService(_postRepository.Object, _userService.Object, _locationRepository.Object, _mapper.Object, null);
        var response = await sut.Create(request);

        Assert.AreEqual(response.Id, 1);
        Assert.AreEqual(response.LocationId, 1);
        Assert.AreEqual(response.Text, "text");

        _postRepository.VerifyAll();
        _userService.VerifyAll();
        _locationRepository.VerifyAll();
        _mapper.VerifyAll();
    }
}