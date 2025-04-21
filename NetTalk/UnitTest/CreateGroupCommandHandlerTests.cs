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
using UnitTest.common;
using Xunit.Categories;

namespace UnitTest;


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
        var testUser = TestHelper.CreateTestUser();
        await repository.AddAsync(testUser);
        await unitOfWork.SaveChangesAsync();
        // Arrange
        var command = new Faker<CreateGroupCommand>()
            .RuleFor(command => command.Name, faker => faker.Person.FullName)
            .Generate();
            command.Users = new HashSet<Guid>();

        var handler = new CreateGroupCommandHandler(unitOfWork,
            new TestUser()
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
    

    [Fact]
    public async Task Add_InvalidUsersAsMembers_ShouldReturnsCreatedResult()
    {
        
        var unitOfWork = new UnitOfWork(
            fixture.DbContext,
            Substitute.For<IMediator>(),
            Substitute.For<IEventStoreRepository>(),
            Substitute.For<ILogger<UnitOfWork>>());
        var repository = new UserRepository(fixture.DbContext);
        var testUser =TestHelper.CreateTestUser();
        await repository.AddAsync(testUser);
        await unitOfWork.SaveChangesAsync();
        // Arrange
        var command = new Faker<CreateGroupCommand>()
            .RuleFor(command => command.Name, faker => faker.Person.FullName)
            .Generate();
        command.Users = new HashSet<Guid>();
        for (var i = 0; i < 3; i++)
        {
            command.Users.Add(TestHelper.CreateTestUser().Id);
        }
        var handler = new CreateGroupCommandHandler(unitOfWork,
            new TestUser()
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