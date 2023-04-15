using AutoMapper;
using P4P.Models;
using P4P.Models.DTOs.Response;
using P4P.Wrappers;

namespace P4P.Profiles;

public class PaginationProfile : Profile
{
    public PaginationProfile()
    {
        CreateMap<PaginatedList<List<Location>>, PaginatedList<List<LocationResponse>>>();
        CreateMap<PaginatedList<List<Post>>, PaginatedList<List<PostResponse>>>();
        CreateMap<PaginatedList<List<Post>>, PaginatedList<List<LocationByIdPostResponse>>>();
    }
}
