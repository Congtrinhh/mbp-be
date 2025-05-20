using Application.Dtos.Admin;
using AutoMapper;
using Domain.Entities;

namespace Application.Admin.Mapping
{
    public class AdminMappingProfile : Profile
    {
        public AdminMappingProfile()
        {
            CreateMap<User, AdminUserDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt))
                .ReverseMap();

            // Add mappings for other entities as needed
            // For example:
            // CreateMap<Post, AdminPostDto>().ReverseMap();
        }
    }
}