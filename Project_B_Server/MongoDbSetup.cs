using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Project_B_Server_Domain;
using Project_B_Server.Services;

namespace Project_B_Server;

public static class MongoDbSetup
{
    public static void AddMongoDb(WebApplicationBuilder builder)
    {
        // MongoDb Services
        builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));

        builder.Services.AddSingleton<MongoClient>(provider =>
            new MongoClient(provider.GetService<IOptions<MongoDbSettings>>()!.Value.ConnectionUri));
        
        builder.Services.AddScoped<MongoDbService<Message>>(provider => 
            new MongoDbService<Message>(
                mongoClient: provider.GetService<MongoClient>()!,
                databaseName: provider.GetService<IOptions<MongoDbSettings>>()!.Value.DatabaseName,
                collectionName: "messages"));
        
        builder.Services.AddScoped<MongoDbService<Client>>(provider => 
            new MongoDbService<Client>(
                mongoClient: provider.GetService<MongoClient>()!,
                databaseName: provider.GetService<IOptions<MongoDbSettings>>()!.Value.DatabaseName,
                collectionName: "clients"));
    }
}