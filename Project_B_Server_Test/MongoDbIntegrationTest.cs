using EphemeralMongo;
using MongoDB.Driver;

namespace Project_B_Server_Test;

/// <summary>
/// Base class for MongoDB integration tests.
/// </summary>
public class MongoDbIntegrationTest : IDisposable
{
    IMongoRunner runner;
    protected readonly MongoClient mongoClient;

    protected MongoDbIntegrationTest()
    {
        var options = new MongoRunnerOptions
        {
            ConnectionTimeout = TimeSpan.FromSeconds(10), // Default: 30 seconds
            ReplicaSetSetupTimeout = TimeSpan.FromSeconds(5), // Default: 10 seconds
            AdditionalArguments = "--quiet", // Default: null,
            //MongoPort = 27017 // setting a static port might cause some issues
        };

        runner = MongoRunner.Run(options);
        mongoClient = new MongoClient(runner.ConnectionString);
        CustomLogger.Log.Information(runner.ConnectionString);
    }

    public void Dispose()
    {
        runner.Dispose();
    }
}