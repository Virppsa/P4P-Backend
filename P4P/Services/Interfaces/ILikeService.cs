using P4P.Filters;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Wrappers;

namespace P4P.Services.Interfaces;

public interface ILikeService
{
    PaginatedList<List<Like>> GetAll(PaginationFilter filter);

    Task<LikeResponse> Create(CreateLikeRequest reaction);
}
