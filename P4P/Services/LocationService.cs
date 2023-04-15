using P4P.Filters;
using P4P.Models;
using P4P.Repositories.Interfaces;
using P4P.Services.Interfaces;
using P4P.Wrappers;
using System.Collections.Concurrent;
using AutoMapper;
using P4P.Exceptions;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;

namespace P4P.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private static readonly ConcurrentDictionary<int, ConcurrentQueue<string>> ConcurrentDictionary = new();
    private readonly IFileService<Location> _fileService;

    public LocationService(
        ILocationRepository locationRepository, 
        IUserService userService, 
        IMapper mapper,
        IPostRepository postRepository, 
        IFileService<Location> fileService
    )
    {
        _locationRepository = locationRepository;
        _userService = userService;
        _mapper = mapper;
        _postRepository = postRepository;
        _fileService = fileService;
    }

    public PaginatedList<List<LocationResponse>> GetAll(LocationFilter filter)
    {
        var paginatedList = _locationRepository.GetPaginated(
            filter, 
            filter: x => x.X > filter.X1 && x.X < filter.X2 && x.Y > filter.Y1 && x.Y < filter.Y2
        );

         return _mapper.Map<PaginatedList<List<LocationResponse>>>(paginatedList);
    }

    public async Task<LocationByIdResponse> GetSingle(int locationId, PaginationFilter filter)
    {
        var requiredEntities = new List<string>() { "User" };

        var paginatedList = _postRepository.GetPaginated(filter, x => x.LocationId == locationId, requiredFields: requiredEntities);

        var mappedPostList = _mapper.Map<PaginatedList<List<LocationByIdPostResponse>>>(paginatedList);

        var location = await _locationRepository.GetByIdAsync(locationId) ??
               throw new HttpException(
                   StatusCodes.Status404NotFound,
                   "Vietovė nerasta"
               );

        var mappedResponse = _mapper.Map<LocationByIdResponse>(location);

        mappedResponse.Posts = mappedPostList;

        return mappedResponse;
    }

    public async Task<LocationResponse> Create(CreateLocationRequest createLocationRequest)
    {
        var location = _mapper.Map<Location>(createLocationRequest);
        location.ImageName = await _fileService.SaveImageAsync(createLocationRequest.Image);
        location = await _locationRepository.InsertAsync(location);
        
        return _mapper.Map<LocationResponse>(location);
    }

    // 8. Concurrent programming (threading or async/await (for your own written classes); common resource usage between threads);
    public async Task<LocationResponse> AddRating(int locationId, LocationRatingRequest locationRatingRequest)
    {
        var user = await _userService.GetCurrent() ?? throw new Exception("UserId cannot be null when adding rating");
        var location = await _locationRepository.GetByIdAsync(locationId) ??
                       throw new HttpException(
                           StatusCodes.Status404NotFound,
                           "Vietovė nerasta"
                       );

        if (!ConcurrentDictionary.ContainsKey(locationId))
        {
            ConcurrentDictionary.TryAdd(locationId, new ConcurrentQueue<string>(location.Ratings));
        }

        var queue = ConcurrentDictionary[locationId];

        if (queue.Any(x => int.Parse(x.Split("-")[1]) == user.Id))
        {
            throw new HttpException(
                StatusCodes.Status400BadRequest,
                "Ši vietovė jau įvertinta"
            );
        }

        location.Ratings = queue.Append($"{locationRatingRequest.Rating}-{user.Id}").ToList();
        location = await _locationRepository.UpdateAsync(location);

        ConcurrentDictionary.Remove(locationId, out _);

        return _mapper.Map<LocationResponse>(location);
    }
}
