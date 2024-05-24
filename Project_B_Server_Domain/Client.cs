using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project_B_Server_Domain;

public class Client
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string? ClientId { get; set; }
    public string? ClientName { get; set; }
    public string? ClientPlayerSprite { get; set; }
    public string? ClientNickName { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }
}