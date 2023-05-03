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
        // You can install the db with mongodb installer, or you can use docker
        
        // THESE COMMANDS ARE FOR INITIAL SETUP OF MONGODB --------------------------------------
        
        // docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo
        // -d --rm doesnt attach - and rm means its destoryed when you stop it. -p is opening a port to map to the container
        // -v is volume, which is a way to persist data. mongodbdata is the name of the volume, and /data/db is the path inside the container
        // docker ps -- checks if its running
        
        // docker exec -it mongo bash
        // mongo
        // show dbs
        // use catalog
        // show collections
        // db.items.find()
        // db.items.insertOne({name: "Potion", price: 9.99})
        // db.items.find()
        // db.items.deleteOne({name: "Potion"})
        
        // Everything is packaged in docker image - when you run it, it will create a container
        // Docker container will run in docker engine
        
        itemsCollection.InsertOne(item);
    }

    public void UpdateItem(Item item)
    {
        throw new NotImplementedException();
    }

    public void DeleteItem(Guid id)
    {
        throw new NotImplementedException();
    }
}