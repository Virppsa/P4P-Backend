using AutoMapper;
using P4P.Models;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;

namespace P4P.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>();
        CreateMap<User, LoginRequest>().ReverseMap();
        CreateMap<User, RegisterRequest>().ReverseMap();
        CreateMap<User, PostUserResponse>();
        CreateMap<User, LocationByIdUserResponse>();
    }
}
