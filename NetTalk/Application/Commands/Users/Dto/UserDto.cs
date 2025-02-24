using AutoMapper;
using Domain.Entities;

namespace Application.Users.Dto;

public class UserDto
{
    public Guid  Id { get; set; }
    public string AvatarUrl { get; set; }
    public string Login { get; set; }
    public string FullName { get; set; }
    public long IdChat{ get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.IdChat, opt => opt.Ignore());
        }
    }
}