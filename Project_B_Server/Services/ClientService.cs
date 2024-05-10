using Project_B_Server_Domain;

namespace Project_B_Server.Services;

public class ClientService(MongoDbService<Client> mongoDbService)
{
    public Task<List<Client>> GetClientsAsync()
    {
        return mongoDbService.GetItemsAsync();
    }
    
    public async Task AddClientAsync(string clientId, string clientName)
    {
        await mongoDbService.CreateItemAsync(new Client
        {
            ClientId = clientId,
            ClientName = clientName
        });
    }
}