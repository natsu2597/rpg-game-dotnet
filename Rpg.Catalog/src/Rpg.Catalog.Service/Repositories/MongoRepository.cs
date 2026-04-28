using MongoDB.Driver;
using Rpg.Catalog.Service.Models;

namespace Rpg.Catalog.Service.Repositories;

public class MongoRepository<T> : IRepository<T> where T : IModel
{

    private readonly IMongoCollection<T> dbCollection;

    private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;


    public MongoRepository(IMongoDatabase database,string collectionName)
    {
        dbCollection = database.GetCollection<T>(collectionName);
    }

    public async Task<IReadOnlyCollection<T>> GetAllItemAsync()
    {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    public async Task<T> GetItemAsync(Guid id)
    {
        FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateItemAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        await dbCollection.InsertOneAsync(item);
    }

    public async Task UpdateItemAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        FilterDefinition<T> filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
        await dbCollection.ReplaceOneAsync(filter, item);
    }

    public async Task DeleteItemAsync(Guid id)
    {
        FilterDefinition<T> filter = filterBuilder.Eq(item => item.Id, id);
        await dbCollection.DeleteOneAsync(filter);
    }


}