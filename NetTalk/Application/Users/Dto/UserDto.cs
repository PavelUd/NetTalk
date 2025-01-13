using AutoMapper;
using Domain.Entities;

namespace Application.Users.Dto;

public class UserDto
{
    public int Id { get; set; }
    public string AvatarUrl { get; set; }
    public string Login { get; set; }
    public string FullName { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDto>();
        }
    }
}