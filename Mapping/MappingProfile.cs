using AutoMapper;
using AwayDayzAPI.Models;
using AwayDayzAPI.Models.DTOs.Auth;
using AwayDayzAPI.Models.DTOs.Friend;
using AwayDayzAPI.Models.DTOs.Stadium;
using AwayDayzAPI.Models.DTOs.User;
using AwayDayzAPI.Models.Responses;
using Microsoft.IdentityModel.Tokens;

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


            // Mappa från StadiumApiResponse till en lista av StadiumDto
            CreateMap<StadiumApiResponse, List<StadiumDto>>()
                .AfterMap((src, dest) =>
                {
                    dest.AddRange(src.Response.Select(r => new StadiumDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Address = r.Address,
                        City = r.City,
                        Country = r.Country,
                        Capacity = r.Capacity,
                        Surface = r.Surface,
                        ImageUrl = r.Image
                    }));
                });

            CreateMap<Stadium, StadiumDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.Surface, opt => opt.MapFrom(src => src.Surface))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<StadiumApiResponse.ResponseStadium, StadiumDto>()
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src =>
                    src.Capacity > 0 ? src.Capacity : 0)) // Sätt till 0 om Capacity är ogiltigt
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.Surface, opt => opt.MapFrom(src => src.Surface))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image));


        }
    }
}
