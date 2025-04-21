using Application.Chat.Dto;
using Application.Commands.Message;
using AutoMapper;
using FluentAssertions;
using Infrastructure.Encryption;
using IntegrationTest.Fixtures;
using MediatR;
using NSubstitute;
using UnitTest.common;

namespace UnitTest.MessageTests;

public class CreateMessageCommandHandlerTests : MessageHandlerTestBase
{
    public CreateMessageCommandHandlerTests(NetTalkWriteDbFixture fixture) : base(fixture)
    {
    }
    
    [Fact]
    public async Task Handle_ValidRequest_ShouldUpdateMessageSuccessfully()
    {
        var idChat = Guid.NewGuid();
        var message = await CreateMessage(idChat, TestUser.Id);
        var command = new CreateMessage()
        {
            IdChat = message.IdChat,
            Text = "test"
        };

        var handler = new AddMessageHandler(
            UnitOfWork,
            UserRepository,
            ChatRepository,
            MessageRepository,
            TestUser,
            new MessageEncryptor()
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnMessageDto()
    {
        var idChat = Guid.NewGuid();
                var message = await CreateMessage(idChat, TestUser.Id);
                var command = new CreateMessage()
                {
                    IdChat = message.IdChat,
                    Text = "test"
                };
        
                var handler = new AddMessageHandler(
                    UnitOfWork,
                    UserRepository,
                    ChatRepository,
                    MessageRepository,
                    TestUser,
                    new MessageEncryptor()
                );
        
                // Assert
                var act = await handler.Handle(command, CancellationToken.None);
                var resultMessage = new MessageDto(message, command.Text, TestUser);
                // Assert
                act.Should().NotBeNull();
                act.Succeeded.Should().BeTrue();
                act.Data.Id.Should().NotBeEmpty();
                act.Data.Should().BeEquivalentTo(resultMessage, options => options.Excluding(x => x.UpdatedDate).Excluding(x => x.CreatedDate).Excluding(x => x.Id));
    }

    [Fact]
    public async Task Handle_ChatNotFound_ShouldReturnFailure()
    {
        var command = new CreateMessage()
        {
            IdChat = Guid.NewGuid(),
            Text = "test"
        };

        var handler = new AddMessageHandler(
            UnitOfWork,
            UserRepository,
            ChatRepository,
            MessageRepository,
            TestUser,
            new MessageEncryptor()
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeFalse();
        act.Errors.Should().BeEquivalentTo("Chat not found");
    }

    [Fact]
    public async Task Handle_EmptyText_ShouldReturnFailure()
    {
        var command = new CreateMessage()
        {
            IdChat = Guid.NewGuid(),
            Text = ""
        };

        var handler = new AddMessageHandler(
            UnitOfWork,
            UserRepository,
            ChatRepository,
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
    public async Task Handle_InvalidChatId_ShouldReturnFailure()
    {
        var command = new CreateMessage()
        {
            Text = "ddd"
        };

        var handler = new AddMessageHandler(
            UnitOfWork,
            UserRepository,
            ChatRepository,
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
    public async Task Handle_EncryptionFails_ShouldReturnFailure()
    {
        var idChat = Guid.NewGuid();
        var message = await CreateMessage(idChat, TestUser.Id);
        var command = new CreateMessage()
        {
            IdChat = message.IdChat,
            Text = "test"
        };
        var failTestUser = new TestUser()
        {
            Id = Guid.NewGuid(),
        };
        var handler = new AddMessageHandler(
            UnitOfWork,
            UserRepository,
            ChatRepository,
            MessageRepository,
            failTestUser,
            new MessageEncryptor()
        );

        // Act
        var act = await handler.Handle(command, CancellationToken.None);
        // Assert
        act.Should().NotBeNull();
        act.Succeeded.Should().BeFalse();
    }
}