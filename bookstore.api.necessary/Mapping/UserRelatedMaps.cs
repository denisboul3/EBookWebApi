using AutoMapper;
using bookstore.api.DTO;
using bookstore.api.Models;
using bookstore.api.necessary.DTO.Projections;

namespace bookstore.api.Mapping;

public class UserModelToUserDtoMappingProfile : Profile
{
    public UserModelToUserDtoMappingProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<UserModel, UserDto>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            //.ForMember(dest => dest.Role, opt => opt.MapFrom(src => src!.Role!.Name))
            .MaxDepth(1)
            .ReverseMap();
    }
}