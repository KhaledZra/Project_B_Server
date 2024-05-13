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

    public async Task AddClientAsync(string clientId, string clientName)
    {
        await mongoDbService.CreateItemAsync(new Client
        {
            ClientId = clientId,
            ClientName = clientName
        });
    }
    
    public async Task DeleteClientAsync(string id)
    {
        await mongoDbService.DeleteItemAsync(id);
    }
}