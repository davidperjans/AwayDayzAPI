using AutoMapper;
using AwayDayzAPI.DTOs;
using AwayDayzAPI.Models;

namespace AwayDayzAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, ProfileDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<FriendRequest, FriendRequestDto>()
                .ForMember(dest => dest.SenderUsername, opt => opt.MapFrom(src => src.Sender != null ? src.Sender.UserName : "Okänd"))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Friendship, FriendListDto>()
                .ForMember(dest => dest.FriendName, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.User1Id == (string)context.Items["CurrentUserId"] ? src.User2.UserName : src.User1.UserName))
                .ForMember(dest => dest.FriendsSince, opt => opt.MapFrom(src => src.StartedAt));
        }
    }
}
