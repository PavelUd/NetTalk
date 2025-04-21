using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using IntegrationTest.Fixtures;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Persistence.Repositories;
using Persistence.Repositories.Write;
using UnitTest.common;

namespace UnitTest.MessageTests;

public abstract class MessageHandlerTestBase : IClassFixture<NetTalkWriteDbFixture>, IAsyncLifetime
{
    protected readonly NetTalkWriteDbFixture Fixture;
    protected readonly UnitOfWork UnitOfWork;
    protected IUser TestUser;
    protected readonly MessageRepository MessageRepository;
    protected readonly ChatRepository ChatRepository;
    protected readonly UserRepository UserRepository;

    protected MessageHandlerTestBase(NetTalkWriteDbFixture fixture)
    {
        Fixture = fixture;
        UnitOfWork = new UnitOfWork(
            Fixture.DbContext,
            Substitute.For<IMediator>(),
            Substitute.For<IEventStoreRepository>(),
            Substitute.For<ILogger<UnitOfWork>>());

        UserRepository = new UserRepository(Fixture.DbContext);
        MessageRepository = new MessageRepository(Fixture.DbContext);
        ChatRepository = new ChatRepository(Fixture.DbContext);
    }
    
    public async Task InitializeAsync()
    {
        var user = TestHelper.CreateTestUser();
        TestUser = new TestUser { Id = user.Id };
        
        await UserRepository.AddAsync(user);
        await UnitOfWork.SaveChangesAsync();
    }
    
    protected async Task<Message> CreateMessage(Guid idChat, Guid idUser)
    {
        var chat = TestHelper.CreateTestChat(idChat);
        var message = TestHelper.CreateTestMessage(chat.Id, idUser);
        await ChatRepository.AddAsync(chat);
        await MessageRepository.AddAsync(message);
        await UnitOfWork.SaveChangesAsync();
        return message;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

}