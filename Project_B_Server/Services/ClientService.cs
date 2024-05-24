using Project_B_Server_Domain;

namespace Project_B_Server.Services;

public class ClientService(MongoDbService<Client> mongoDbService)
{
    public async Task<List<Client>> GetClientsAsync()
    {
        return await mongoDbService.GetItemsAsync();
    }
    
    public async Task<Client?> GetClientWithClientIdAsync(string clientId)
    {
        List<Client> result = await mongoDbService.GetItemsWithCustomFilterAsync(
            "ClientId", clientId, MongoFilters.Equal);
        if (result.Count == 0) return null;
        
        return result.First();
    }

    public async Task AddClientAsync(string clientId, string clientName, string clientPlayerSprite, string clientNickName, float positionX, float positionY)
    {
        await mongoDbService.CreateItemAsync(new Client
        {
            ClientId = clientId,
            ClientName = clientName,
            ClientPlayerSprite = clientPlayerSprite,
            ClientNickName = clientNickName,
            PositionX = positionX,
            PositionY = positionY
        });
    }

    public async Task UpdateClientPositionAsync(string clientName, float positionX, float positionY)
    {
        Client client = await mongoDbService.GetItemWithCustomFilterAsync("ClientName", clientName);
        
        client.PositionX = positionX;
        client.PositionY = positionY;

        await mongoDbService.ReplaceItemAsync(client, client.Id!);
    }
    
    public async Task DeleteClientAsync(string id)
    {
        await mongoDbService.DeleteItemAsync(id);
    }
}