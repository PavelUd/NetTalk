
using System.Data.Common;
using System.Net.Http.Headers;
using Application.Commands.Chat.Create;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Repositories.Query;
using Application.Common.Result;
using Bogus;
using FluentAssertions;
using IntegrationTest.Extensions;
using IntegrationTest.Fixtures;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using NSubstitute;
using Persistence.Contexts;
using Xunit.Categories;
namespace IntegrationTest.Controllers;

[Collection("Database")]
[IntegrationTest]
public class GroupControllerTests : IAsyncLifetime
{
    private NetTalkDbContext testDbContext;
    private const string ConnectionString = "Host=localhost;Port=5432;Database=net-talk-db;Username=postgres;Password=root";
    private const string Endpoint = "/api/chats";
    private readonly NpgsqlConnection _writeDbContext = new(ConnectionString);

    public GroupControllerTests(NetTalkWriteDbFixture databaseFixture)
    {
        testDbContext = databaseFixture.DbContext;
    }
    #region POST: /api/chats/

    [Fact]
    public async Task Should_ReturnsHttpStatus201Created_When_Post_ValidRequest()
    {
        // Arrange
        await using var webApplicationFactory = InitializeWebAppFactory();
        using var httpClient = webApplicationFactory.CreateClient(CreateClientOptions());
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6Ijk5Yzk1YjI2LWFiMjktNDEzNi04YjlmLTM1NDc0OWFjY2Y4YiIsInVuaXF1ZV9uYW1lIjoicEBwZ21haWwuY29tIiwiRnVsbE5hbWUiOiJkYXJ6YSIsIlBob3RvVXJsIjoiaHR0cHM6Ly9tZGJjZG4uYi1jZG4ubmV0L2ltZy9QaG90b3MvbmV3LXRlbXBsYXRlcy9ib290c3RyYXAtY2hhdC9hdmEyLWJnLndlYnAiLCJuYmYiOjE3NDA2OTUxODgsImV4cCI6MTc0MDY5ODc4OCwiaWF0IjoxNzQwNjk1MTg4fQ.FaNQl8-24bDKgh3e8OtwGAR2_968E3sn1wL51sQSOYU");
        var command = new Faker<CreateChatCommand>()
            .RuleFor(command => command.Name, faker => faker.Person.FullName)
            .RuleFor(command => command.Type, faker => "group")
            .RuleFor(command => command.Users, new List<Guid>())
            .Generate();
        
        using var jsonContent = command.ToJsonHttpContent();
        using var act = await httpClient.PostAsync(Endpoint, jsonContent);
        
        act.Should().NotBeNull();
        act.IsSuccessStatusCode.Should().BeTrue();
        
        var response = JsonConvert.DeserializeObject<Result<Guid>>(await act.Content.ReadAsStringAsync());
        response.Should().NotBeNull();
        response.Succeeded.Should().BeTrue();
        response.Errors.Should().BeEmpty();
        response.Data.Should().NotBeEmpty();
        
    }
    
    #endregion
    
    public async Task InitializeAsync()
    {
        await _writeDbContext.OpenAsync();
    }

    public async Task DisposeAsync()
    {
        await _writeDbContext.DisposeAsync();
    }
    
    private WebApplicationFactory<Program> InitializeWebAppFactory(
    Action<IServiceCollection> configureServices = null,
    Action<IServiceScope> configureServiceScope = null)
{
    return new WebApplicationFactory<Program>()
        .WithWebHostBuilder(hostBuilder =>
        {
            hostBuilder.UseSetting("ConnectionStrings:SqlConnection", "Host=localhost;Port=5432;Database=net-talk-db;Username=postgres;Password=root");
            hostBuilder.UseSetting("ConnectionStrings:NoSqlConnection", "mongodb://localhost:27017/");

            hostBuilder.UseEnvironment("Testing");

            hostBuilder.ConfigureLogging(logging => logging.ClearProviders());

            hostBuilder.ConfigureServices(services =>
            {
                services.RemoveAll<DbConnection>();
                services.RemoveAll<DbContextOptions>();
                services.RemoveAll<NetTalkDbContext>();
                services.RemoveAll<DbContextOptions<NetTalkDbContext>>();
                services.RemoveAll<ReadNetTalkDbContext>();
                services.RemoveAll<ISynchronizeDb>();

                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<NetTalkDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddSingleton(testDbContext);

                services.AddSingleton(_ => Substitute.For<IReadDbContext>());
                services.AddSingleton(_ => Substitute.For<ISynchronizeDb>());

                configureServices?.Invoke(services);

                using var serviceProvider = services.BuildServiceProvider(true);
                using var serviceScope = serviceProvider.CreateScope();
                var writeDbContext = serviceScope.ServiceProvider.GetRequiredService<NetTalkDbContext>();
                writeDbContext.Database.EnsureCreated();

                configureServiceScope?.Invoke(serviceScope);
            });
        });
}
     
    [Fact]
    public void CreateAndDropDatabase()
    {
        Assert.True(true);
    }
     
    private static WebApplicationFactoryClientOptions CreateClientOptions() => new() { AllowAutoRedirect = false };
}