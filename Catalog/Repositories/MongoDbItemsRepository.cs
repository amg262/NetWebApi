using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories;

public class MongoDbItemsRepository : IItemsRepository
{
    // private readonly List<Item> items = new()

    // Saving info to connect to db
    private const string DatabaseName = "catalog";
    private const string CollectionName = "items";
    private readonly IMongoCollection<Item> _itemsCollection;

    private readonly FilterDefinitionBuilder<Item>
        _filterBuilder = Builders<Item>.Filter; // This is a helper to build filters

    public MongoDbItemsRepository(IMongoClient mongoClient)
    {
        // Driver will create db if it doesnt exist
        // Driver will create collection if it doesnt exist
        // Doesnt matter which API we use, they all use the same driver
        IMongoDatabase database = mongoClient.GetDatabase(DatabaseName); //gets reference to db
        _itemsCollection = database.GetCollection<Item>(CollectionName); //gets reference to collection
    }


    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
        // Find() returns a cursor, which is a pointer to the data
        return await _itemsCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<Item?> GetItemAsync(Guid id)
    {
        var filter = _filterBuilder.Eq(item => item.Id, id); // Filter that will return the item with matching id
        return await _itemsCollection.Find(filter)
            .SingleOrDefaultAsync(); // This will return the item with the matching id
    }

    public async Task CreateItemAsync(Item item)
    {
        await _itemsCollection.InsertOneAsync(item);
    }

    public async Task UpdateItemAsync(Item item)
    {
        var filter =
            _filterBuilder.Eq(existingItem => existingItem.Id,
                item.Id); // Filter that will return the item with matching id
        await _itemsCollection.ReplaceOneAsync(filter, item); // This will replace the item with the matching id
    }

    public async Task DeleteItemAsync(Guid id)
    {
        var filter = _filterBuilder.Eq(item => item.Id, id); // Filter that will return the item with matching id
        await _itemsCollection.DeleteOneAsync(filter); // This will delete the item with the matching id
    }
}