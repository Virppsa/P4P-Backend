using AutoMapper;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;

namespace P4P.Profiles;

public class LocationProfile : Profile
{
    public LocationProfile()
    {
        CreateMap<Location, LocationResponse>();
        CreateMap<Location, CreateLocationRequest>().ReverseMap()
            .ForMember(x => x.ImageName, opt => opt.Ignore());
        CreateMap<Location, PostLocationResponse>();
        CreateMap<Location, LocationByIdResponse>()
            .ForMember(x => x.Posts, opt => opt.Ignore());
    }
}
