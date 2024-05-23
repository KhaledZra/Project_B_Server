using FluentAssertions;
using Project_B_Server_Domain;
using Project_B_Server.Services;

namespace Project_B_Server_Test.ServiceTests;

public class ClientServiceTests : MongoDbIntegrationTest, IDisposable
{
    private ClientService clientService;

    // before each
    public ClientServiceTests()
    {
        MongoDbService<Client> mongoDbService = new MongoDbService<Client>(base.mongoClient, "test_db", "clients");
        clientService = new ClientService(mongoDbService);
    }

    // after each
    public new void Dispose()
    {
        base.Dispose();
    }

    [Fact]
    public async Task ShouldGenerateUniqueBsonIdTest()
    {
        // Arrange
        await clientService.AddClientAsync("1", "client1", "player_1", 0, 0);

        // Act
        var result = await clientService.GetClientsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);

        result[0].Id.Should().NotBeNullOrEmpty();
    }


    [Fact]
    public async Task ShouldGetCreatedClientsTest()
    {
        // Arrange
        await clientService.AddClientAsync("1", "client1", "player_1", 0, 0);
        await clientService.AddClientAsync("2", "client2", "player_2", 0, 0);

        // Act
        var result = await clientService.GetClientsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result[0].ClientId.Should().Be("1");
        result[0].ClientName.Should().Be("client1");
        result[0].ClientPlayerSprite.Should().Be("player_1");
        result[0].PositionX.Should().Be(0);
        result[0].PositionY.Should().Be(0);

        result[1].ClientId.Should().Be("2");
        result[1].ClientName.Should().Be("client2");
        result[1].ClientPlayerSprite.Should().Be("player_2");
        result[1].PositionX.Should().Be(0);
        result[1].PositionY.Should().Be(0);
    }

    [Fact]
    public async Task ShouldGetClientWithClientIdTest()
    {
        // Arrange
        await clientService.AddClientAsync("1", "client1", "player_1", 0, 0);
        var createdClient = await clientService.GetClientsAsync();

        // Act
        Client? result = await clientService.GetClientWithClientIdAsync(createdClient.First().ClientId!);

        // Assert
        result.Should().NotBeNull();

        result!.ClientId.Should().Be("1");
        result.ClientName.Should().Be("client1");
        result.ClientPlayerSprite.Should().Be("player_1");
        result.PositionX.Should().Be(0);
        result.PositionY.Should().Be(0);
    }

    [Fact]
    public async Task ShouldUpdateExistingClientPositionTest()
    {
        // Arrange
        await clientService.AddClientAsync("1", "client1", "player_1", 0, 0);
        var createdClient = await clientService.GetClientsAsync().ContinueWith(t => t.Result.First());
        await clientService.UpdateClientPositionAsync(createdClient.ClientName!, 50.0f, 50.0f);

        // Act
        var updatedClient = await clientService.GetClientsAsync().ContinueWith(t => t.Result.First());

        // Assert
        updatedClient.Should().NotBeNull();

        updatedClient!.ClientId.Should().Be("1");
        updatedClient.ClientName.Should().Be("client1");
        updatedClient.ClientPlayerSprite.Should().Be("player_1");
        updatedClient.PositionX.Should().Be(50.0f);
        updatedClient.PositionY.Should().Be(50.0f);
    }

    [Fact]
    public async Task ShouldDeleteExistingClientTest()
    {
        // Arrange
        await clientService.AddClientAsync("1", "client1", "player_1", 0, 0);
        var createdClient = await clientService.GetClientsAsync().ContinueWith(t => t.Result.First());
        await clientService.DeleteClientAsync(createdClient.Id!);

        // Act
        var result = await clientService.GetClientsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}