namespace Project_B_Server_Domain;

public class MongoDbSettings
{
    public string ConnectionUri { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string CollectionName { get; set; } = null!;
}