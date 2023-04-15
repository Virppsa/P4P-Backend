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
public class LocationServiceTest
{
    private readonly Mock<ILocationRepository> _locationRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IUserService> _userService;
    private readonly Mock<IFileService<Location>> _fileService;
    private readonly Mock<IPostRepository> _postRepository;

    public LocationServiceTest()
    {
        _locationRepository = new Mock<ILocationRepository>();
        _mapper = new Mock<IMapper>();
        _userService = new Mock<IUserService>();
        _fileService = new Mock<IFileService<Location>>();
        _postRepository = new Mock<IPostRepository>();
    }

    [TestMethod]
    public async Task GetAll()
    {
        var filter = new LocationFilter();

        _locationRepository
            .Setup(x => x.GetPaginated(
                It.IsAny<PaginationFilter>(),
                It.IsAny<Expression<Func<Location, bool>>>(),
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                It.IsAny<List<string>>()))
            .Returns(new PaginatedList<List<Location>>());

        _mapper
            .Setup(x => x.Map<PaginatedList<List<LocationResponse>>>(
                It.IsAny<PaginatedList<List<Location>>>()))
            .Returns(new PaginatedList<List<LocationResponse>>());

        var sut = new LocationService(_locationRepository.Object, null, _mapper.Object, _postRepository.Object, _fileService.Object);
        var response = sut.GetAll(filter);

        Assert.AreEqual(response.GetType(), typeof(PaginatedList<List<LocationResponse>>));

        _locationRepository.VerifyAll();
        _mapper.VerifyAll();
    }

    [TestMethod]
    public async Task GetSingle()
    {
        _locationRepository
            .Setup(x => x.GetByIdAsync(1))
            .Returns(Task.FromResult(new Location
            {
                Id = 1,
                Description = "desc",
                Name = "name",
                X = 0,
                Y = 0,
            }));

        _mapper
            .Setup(x => x.Map<LocationByIdResponse>(
                It.Is<Location>(r =>
                    r.Id == 1 &&
                    r.Description == "desc" &&
                    r.Name == "name" &&
                    r.X == 0 &&
                    r.Y == 0)))
            .Returns(new LocationByIdResponse
            {
                Id = 1,
                Description = "desc",
                Name = "name",
                X = 0,
                Y = 0,
            });

        var sut = new LocationService(_locationRepository.Object, null, _mapper.Object, _postRepository.Object, _fileService.Object);
        var response = await sut.GetSingle(1, new PaginationFilter());

        Assert.AreEqual(response.Name, "name");
        Assert.AreEqual(response.Id, 1);
        Assert.AreEqual(response.X, 0);
        Assert.AreEqual(response.Y, 0);

        _locationRepository.VerifyAll();
        _mapper.VerifyAll();
    }

    [TestMethod]
    public async Task Create()
    {
        var request = new CreateLocationRequest()
        {
            Name = "name",
            Description = "desc",
            X = 0,
            Y = 0,
        };

        _mapper
            .Setup(x => x.Map<Location>(
                It.Is<CreateLocationRequest>(r =>
                    r.Name == "name" &&
                    r.Description == "desc" &&
                    r.X == 0 &&
                    r.Y == 0)))
            .Returns(new Location
            {
                Id = 1,
                Name = "name",
            });

        _locationRepository
            .Setup(x => x.InsertAsync(
                It.Is<Location>(r =>
                r.Id == 1 &&
                r.Name == "name")))
            .Returns(Task.FromResult(new Location
            {
                Id = 1,
                Name = "name",
            }));

        _mapper
            .Setup(x => x.Map<LocationResponse>(
                It.Is<Location>(r =>
                    r.Id == 1 &&
                    r.Name == "name")))
            .Returns(new LocationResponse
            {
                Id = 1,
                Name = "name",
            });

        var sut = new LocationService(_locationRepository.Object, null, _mapper.Object, _postRepository.Object, _fileService.Object);
        var response = await sut.Create(request);

        Assert.AreEqual(response.Id, 1);
        Assert.AreEqual(response.Name, "name");

        _locationRepository.VerifyAll();
        _mapper.VerifyAll();
    }

    [TestMethod]
    public async Task AddRating()
    {
        var request = new LocationRatingRequest()
        {
            Rating = 1,
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
                Name = "name",
            }));

        _locationRepository
            .Setup(x => x.UpdateAsync(It.Is<Location>(r =>
                    r.Id == 1 &&
                    r.Name == "name")))
            .Returns(Task.FromResult(new Location
            {
                Id = 1,
                Name = "name",
            }));

        _mapper
            .Setup(x => x.Map<LocationResponse>(
                It.Is<Location>(r =>
                    r.Id == 1 &&
                    r.Name == "name")))
            .Returns(new LocationResponse
            {
                Id = 1,
                Name = "name",
            });

        var sut = new LocationService(_locationRepository.Object, _userService.Object, _mapper.Object, _postRepository.Object, _fileService.Object);
        var response = await sut.AddRating(1, request);

        Assert.AreEqual(response.Id, 1);
        Assert.AreEqual(response.Name, "name");

        _userService.VerifyAll();
        _locationRepository.VerifyAll();
        _mapper.VerifyAll();
    }
}