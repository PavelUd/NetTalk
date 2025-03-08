using Application.Commands.Chat.Create;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using IntegrationTest.Fixtures;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Persistence.Repositories;
using Xunit.Categories;

namespace UnitTest;

public class UserTests : IUser
{
    public Guid Id { get; init; }
    public string Name { get; }
    public string AvatarUrl { get; init; }
}

[UnitTest]
public class CreateGroupCommandHandlerTests(NetTalkWriteDbFixture fixture) : IClassFixture<NetTalkWriteDbFixture>
{

    [Fact]
    public async Task Add_ValidCommand_ShouldReturnsCreatedResult()
    {
        var unitOfWork = new UnitOfWork(
            fixture.DbContext,
            Substitute.For<IMediator>(),
            Substitute.For<IEventStoreRepository>(),
            Substitute.For<ILogger<UnitOfWork>>());
        var repository = new UserRepository(fixture.DbContext);
        var testUser = CreateTestUser();
        await repository.AddAsync(testUser);
        await unitOfWork.SaveChangesAsync();
        // Arrange
        var command = new Faker<CreateGroupCommand>()
            .RuleFor(command => command.Name, faker => faker.Person.FullName)
            .Generate();
            command.Users = new HashSet<Guid>();

        var handler = new CreateGroupCommandHandler(unitOfWork,
            new UserTests
            {
                Id = testUser.Id,
            },
            repository,
            new ChatRepository(fixture.DbContext)
            );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeTrue(); ;
        act.Data.Should().NotBe(Guid.Empty);
    }

    private User CreateTestUser()
    {
       var testUser =  new Faker<User>()
            .RuleFor(u => u.Login, f => f.Name.FirstName())
            .RuleFor(u => u.FullName, f => f.Person.FullName)
            .Generate();
        testUser.Id = Guid.NewGuid();
        testUser.Password = "Test";
        testUser.Salt = "Test";
        testUser.AvatarUrl = "Test";
        return testUser;
    }

    [Fact]
    public async Task Add_InvalidUsersAsMembers_ShouldReturnsCreatedResult()
    {
        
        var unitOfWork = new UnitOfWork(
            fixture.DbContext,
            Substitute.For<IMediator>(),
            Substitute.For<IEventStoreRepository>(),
            Substitute.For<ILogger<UnitOfWork>>());
        var repository = new UserRepository(fixture.DbContext);
        var testUser = CreateTestUser();
        await repository.AddAsync(testUser);
        await unitOfWork.SaveChangesAsync();
        // Arrange
        var command = new Faker<CreateGroupCommand>()
            .RuleFor(command => command.Name, faker => faker.Person.FullName)
            .Generate();
        command.Users = new HashSet<Guid>();
        for (var i = 0; i < 3; i++)
        {
            command.Users.Add(CreateTestUser().Id);
        }
        var handler = new CreateGroupCommandHandler(unitOfWork,
            new UserTests
            {
                Id = testUser.Id,
            },
            repository,
            new ChatRepository(fixture.DbContext)
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        
        act.Should().NotBeNull();
        act.Succeeded.Should().BeFalse();
        act.Errors.Should().NotBeNullOrEmpty().And.OnlyHaveUniqueItems();
    }
    
}