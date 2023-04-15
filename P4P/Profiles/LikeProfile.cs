using AutoMapper;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;

namespace P4P.Profiles;

public class LikeProfile : Profile
{
    public LikeProfile()
    {
        CreateMap<Like, CreateLikeRequest>().ReverseMap();
        CreateMap<Like, LikeResponse>()
            .ForMember(x => x.PostId, opt => opt.AllowNull())
            .ForMember(x => x.CommentId, opt => opt.AllowNull());
        CreateMap<Like, PostLikeResponse>()
            .ForMember(x => x.HasLiked, opt => opt.AllowNull())
            .ForMember(x => x.LikeCount, opt => opt.AllowNull());
    }
}
