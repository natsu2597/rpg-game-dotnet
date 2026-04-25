using MongoDB.Driver;
using Rpg.Catalog.Service.Models;

namespace Rpg.Catalog.Service.Repositories;

public class ItemsRepository
{
    private const string collectionName = "items";

    private readonly IMongoCollection<Item> dbCollection;

    private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
    

    public ItemsRepository()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var database = mongoClient.GetDatabase("Catalog");
        dbCollection = database.GetCollection<Item>(collectionName);
    }

    public async Task<IReadOnlyCollection<Item>> GetAllItemAsync()
    {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    public async Task<Item> GetItemAsync(Guid id)
    {
        FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateItemAsync(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        await dbCollection.InsertOneAsync(item);
    }

    public async Task UpdateItemAsync(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        FilterDefinition<Item> filter = filterBuilder.Eq(existingItem =>  existingItem.Id, item.Id);
        await dbCollection.ReplaceOneAsync(filter,item);
    }

    public async Task DeleteItemAsync(Guid id)
    {
        FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
        await dbCollection.DeleteOneAsync(filter);
    }


}