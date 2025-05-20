using Application.Dtos;
using Application.Dtos.IdVerification;
using Application.Dtos.User;
using Domain.Entities;
using System;
using System.Linq;

namespace Application.Mapper
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            // Entity to DTO
            CreateMap<User, UserDto>();
            // DTO to Entity
            CreateMap<UserDto, User>();

            CreateMap<PostDto, Post>();
            CreateMap<Post, PostDto>();

            CreateMap<NotificationDto, Notification>();
            CreateMap<Notification, NotificationDto>();

            CreateMap<MediaDto, Media>();
            CreateMap<Media, MediaDto>();

            CreateMap<ContractDto, Contract>();
            CreateMap<Contract, ContractDto>();

            CreateMap<ClientReviewMcDto, ClientReviewMc>();
            CreateMap<ClientReviewMc, ClientReviewMcDto>();

            CreateMap<McReviewClientDto, McReviewClient>();
            CreateMap<McReviewClient, McReviewClientDto>();


            CreateMap<UserIdVerification, UserIdVerificationDto>();
            CreateMap< UserIdVerificationDto, UserIdVerification>();

            CreateMap<IdInfo, IdInfoDto>();
            CreateMap< IdInfoDto, IdInfo>();
        }
    }
}
