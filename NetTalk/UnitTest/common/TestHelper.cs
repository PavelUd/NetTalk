using Bogus;
using Domain.Entities;
using Infrastructure.Encryption;

namespace UnitTest.common;

public static class TestHelper
{
    public static User CreateTestUser()
    {
        var id = Guid.NewGuid();
        var key = GenerateSymmetricKey();
        var testUser =  new Faker<User>()
            .RuleFor(u => u.Login, f => f.Name.FirstName())
            .RuleFor(u => u.FullName, f => f.Person.FullName)
            .Generate();
        testUser.Id = id;
        testUser.Password = "Test";
        testUser.Salt = "Test";
        testUser.AvatarUrl = "Test";
        testUser.Key = key;
        return testUser;
    }

    public static Chat CreateTestChat(Guid newId = default)
    {
        return  new Chat()
        {
            Id = newId,
            Name = "test",
            Type = "Group"
        };
    }

    public static Message CreateTestMessage(Guid chatId, Guid userId)
    {
        return new Message()
        {
            Id = Guid.NewGuid(),
            IdChat = chatId,
            IdUser = userId,
            Text = []
        };
    }

    private static SymmetricKey GenerateSymmetricKey()
    {
        var encryptor = new SymmetricKeyEncryptor();
        return encryptor.GenerateKey();
    }
}