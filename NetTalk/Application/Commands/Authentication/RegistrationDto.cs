using AutoMapper;
using Domain.Entities;

namespace Application.Commands.Authentication;

public class RegistrationDto
{
    public string Email { get; set; }
    public string Id { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Registration, RegistrationDto>();
        }
    }
}