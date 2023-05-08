using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories;

public class MongoDbItemsRepository : IItemsRepository
{
    // private readonly List<Item> items = new()

    // Saving info to connect to db
    private const string databaseName = "catalog";
    private const string collectionName = "items";
    private readonly IMongoCollection<Item> itemsCollection;
    private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter; // This is a helper to build filters
    public MongoDbItemsRepository(IMongoClient mongoClient)
    {
        // Driver will create db if it doesnt exist
        // Driver will create collection if it doesnt exist
        // Doesnt matter which API we use, they all use the same driver
        IMongoDatabase database = mongoClient.GetDatabase(databaseName); //gets reference to db
        itemsCollection = database.GetCollection<Item>(collectionName); //gets reference to collection
    }


    public IEnumerable<Item> GetItems()
    {
        // Find() returns a cursor, which is a pointer to the data
        return itemsCollection.Find(new BsonDocument()).ToList(); 
    }

    public Item GetItem(Guid id)
    {
        var filter = filterBuilder.Eq(item => item.Id, id); // Filter that will return the item with matching id
        return itemsCollection.Find(filter).SingleOrDefault(); // This will return the item with the matching id
    }

    public void CreateItem(Item item)
    {
        itemsCollection.InsertOne(item);
    }

    public void UpdateItem(Item item)
    {
        var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id); // Filter that will return the item with matching id
        itemsCollection.ReplaceOne(filter, item); // This will replace the item with the matching id
    }

    public void DeleteItem(Guid id)
    {
        var filter = filterBuilder.Eq(item => item.Id, id); // Filter that will return the item with matching id
        itemsCollection.DeleteOne(filter); // This will delete the item with the matching id
    }
}