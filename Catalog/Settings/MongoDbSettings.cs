namespace Catalog.Settings;

public class MongoDbSettings
{
    // Name of the database
    public string Host { get; set; }

    // MongoDb uses port 27017 by default
    public int Port { get; set; }

    // Calculate the connection string needed to connect to the db
    public string ConnectionString => $"mongodb://{Host}:{Port}";
}