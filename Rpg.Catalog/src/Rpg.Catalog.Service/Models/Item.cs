using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Rpg.Catalog.Service.Models;

public class Item
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}