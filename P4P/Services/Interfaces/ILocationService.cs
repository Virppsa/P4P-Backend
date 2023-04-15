using P4P.Filters;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Wrappers;

namespace P4P.Services.Interfaces;

public interface ILocationService
{
    PaginatedList<List<LocationResponse>> GetAll(LocationFilter filter);

    Task<LocationByIdResponse> GetSingle(int locationId, PaginationFilter filter);

    Task<LocationResponse> Create(CreateLocationRequest createLocationRequest);

    Task<LocationResponse> AddRating(int locationId, LocationRatingRequest locationRatingRequest);
}
