using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project_B_Server_Domain;

public class Message
{
    [BsonId, BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? SalonName { get; set; }
    public int SalonSeatAmount { get; set; }
}