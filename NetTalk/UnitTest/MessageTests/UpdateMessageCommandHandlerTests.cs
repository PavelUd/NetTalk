using Application.Chat.Dto;
using Application.Commands.Message;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Encryption;
using IntegrationTest.Fixtures;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Persistence.Repositories;
using Persistence.Repositories.Write;
using UnitTest.common;
using Xunit.Categories;

namespace UnitTest.MessageTests;

[UnitTest]
public class UpdateMessageCommandHandlerTests : MessageHandlerTestBase
{
    public UpdateMessageCommandHandlerTests(NetTalkWriteDbFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldUpdateMessageSuccessfully()
    {
        var idChat = Guid.NewGuid();
        var message = await CreateMessage(idChat, TestUser.Id);
        // Arrange
        var command = new UpdateMessage
        {
            IdMessage = message.Id,
            IdChat = idChat,
            Text = "test"
        };

        var handler = new UpdateMessageHandler(
            UnitOfWork,
            ChatRepository,
            UserRepository,
            MessageRepository,
            TestUser,
            new MessageEncryptor()
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeTrue();
        act.Data.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnUpdatedMessageDto()
    {
        var idChat = Guid.NewGuid();
        var message = await CreateMessage(idChat, TestUser.Id);
        // Arrange
        var command = new UpdateMessage
        {
            IdMessage = message.Id,
            IdChat = idChat,
            Text = "test"
        };

        var handler = new UpdateMessageHandler(
            UnitOfWork,
            ChatRepository,
            UserRepository,
            MessageRepository,
            TestUser,
            new MessageEncryptor()
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        var resultMessage = new MessageDto(message, command.Text, TestUser);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeTrue();
        act.Data.Should().BeEquivalentTo(resultMessage);
        
    }

    [Fact]
    public async Task Handle_MessageNotFound_ShouldReturnFailure()
    {
        var idChat = Guid.NewGuid();
        var message = await CreateMessage(idChat, Guid.NewGuid());
        var command = new UpdateMessage
        {
            IdMessage = message.Id,
            IdChat = message.IdChat,
            Text = "test"
        };

        var handler = new UpdateMessageHandler(
            UnitOfWork,
            ChatRepository,
            UserRepository,
            MessageRepository,
            TestUser,
            new MessageEncryptor()
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeFalse();
        act.Errors.Should().BeEquivalentTo("You can't update this message");
    }
    
    [Fact]
    public async Task Handle_ChatIdMismatch_ShouldReturnFailure()
    {
        var idChat = Guid.NewGuid();
        var message = await CreateMessage(Guid.NewGuid(), TestUser.Id);
        var command = new UpdateMessage
        {
            IdMessage = message.Id,
            IdChat = idChat,
            Text = "test"
        };

        var handler = new UpdateMessageHandler(
            UnitOfWork,
            ChatRepository,
            UserRepository,
            MessageRepository,
            TestUser,
            new MessageEncryptor()
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeFalse();
        act.Errors.Should().BeEquivalentTo("You can't update this message");
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_ShouldReturnFailure()
    {
        var idChat = Guid.NewGuid();
        var message = await CreateMessage(idChat, TestUser.Id);
        var command = new UpdateMessage
        {
        };

        var handler = new UpdateMessageHandler(
            UnitOfWork,
            ChatRepository,
            UserRepository,
            MessageRepository,
            TestUser,
            new MessageEncryptor()
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeFalse();
        act.Errors.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Handle_UserNotOwner_ShouldReturnFailure()
    {
        var command = new UpdateMessage
        {
            IdMessage = Guid.NewGuid(),
            IdChat = Guid.NewGuid(),
            Text = "test"
        };

        var handler = new UpdateMessageHandler(
            UnitOfWork,
            ChatRepository,
            UserRepository,
            MessageRepository,
            TestUser,
            new MessageEncryptor()
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeFalse();
        act.Errors.Should().BeEquivalentTo("Message not found");
    }
}