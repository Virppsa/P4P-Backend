using AutoMapper;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;

namespace P4P.Profiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostResponse>()
            .ForMember(x => x.Like, opt => opt.Ignore());
        CreateMap<Post, CreatePostRequest>().ReverseMap();
        CreateMap<Post, LocationByIdPostResponse>();
    }
}
