using Application.Interfaces;
using Bogus;
using Domain.Entities;

namespace UnitTest.common;

public class TestUser : IUser
{
    public Guid Id { get; init; }
    public string Name { get; }
    public string AvatarUrl { get; init; }
    
}