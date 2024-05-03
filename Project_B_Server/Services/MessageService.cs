using Project_B_Server_Domain;

namespace Project_B_Server.Services;

public class MessageService(MongoDbService<Message> mongoDbService)
{
    public Task<List<Message>> GetMessages()
    {
        return mongoDbService.GetItemsAsync();
    }
    
    public async Task AddMessageAsync(string user, string text, DateTime dateStamp)
    {
        await mongoDbService.CreateItemAsync(new Message
        {
            User = user,
            Text = text,
            DateStamp = dateStamp
        });
    }
}