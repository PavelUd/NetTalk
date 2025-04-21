using Application.Commands.Message;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentAssertions;
using IntegrationTest.Fixtures;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Persistence.Repositories;
using Persistence.Repositories.Write;
using UnitTest.common;

namespace UnitTest.MessageTests;

public class DeleteMessageCommandHandlerTests : MessageHandlerTestBase
{
    public DeleteMessageCommandHandlerTests(NetTalkWriteDbFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldUpdateMessageSuccessfully()
    {
        var idChat = Guid.NewGuid();
        var message = await CreateMessage(idChat, TestUser.Id);
        var command = new DeleteMessage()
        {
            IdMessage = message.Id,
            IdChat = message.IdChat,
        };

        var handler = new DeleteMessageHandler(
            MessageRepository,
            ChatRepository,
            TestUser,
            UnitOfWork
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_MessageNotFound()
    {
        var idChat = Guid.NewGuid();
        var command = new DeleteMessage()
        {
            IdMessage = Guid.NewGuid(),
            IdChat = Guid.NewGuid(),
        };

        var handler = new DeleteMessageHandler(
            MessageRepository,
            ChatRepository,
            TestUser,
            UnitOfWork
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeFalse();
        act.Errors.Should().BeEquivalentTo("Message not found");
    }
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenMessageDoesNotBelongToChat()
    {
        var idChat = Guid.NewGuid();
        var message = await CreateMessage(idChat, TestUser.Id);
        var command = new DeleteMessage()
        {
            IdMessage = message.Id,
            IdChat = Guid.NewGuid(),
        };
var failTestUser = new TestUser()
        {
            Id = Guid.NewGuid(),
        };
        var handler = new DeleteMessageHandler(
            MessageRepository,
            ChatRepository,
            TestUser,
            UnitOfWork
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeFalse();
        act.Errors.Should().BeEquivalentTo("You cannot delete this message");
    }
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserIsNotOwnerOfMessage ()
    {
        var idChat = Guid.NewGuid();
        var message = await CreateMessage(idChat, TestUser.Id); 
        var failTestUser = new TestUser()
        {
            Id = Guid.NewGuid(),
        };
        
        var command = new DeleteMessage()
        {
            IdMessage = message.Id,
            IdChat = Guid.NewGuid(),
        };

        var handler = new DeleteMessageHandler(
            MessageRepository,
            ChatRepository,
            failTestUser,
            UnitOfWork
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeFalse();
        act.Errors.Should().BeEquivalentTo("You cannot delete this message");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
    {
        var command = new DeleteMessage();

        var handler = new DeleteMessageHandler(
            MessageRepository,
            ChatRepository,
            TestUser,
            UnitOfWork
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeFalse();
        act.Errors.Should().NotBeEmpty();
    }
}