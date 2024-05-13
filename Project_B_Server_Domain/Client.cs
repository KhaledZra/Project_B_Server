using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project_B_Server_Domain;

public class Client
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string? ClientId { get; set; }
    public string? ClientName { get; set; }
}