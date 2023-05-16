namespace Catalog.Settings;

public class MongoDbSettings
{
    // Localhost is the default host
    public string Host { get; set; }

    // MongoDb uses port 27017 by default
    public int Port { get; set; }

    public string User { get; set; } // mongoadmin
    
    public string Password { get; set; } // password

    // Calculate the connection string needed to connect to the db
    public string ConnectionString => $"mongodb://{Host}:{Port}";
    
    // Calculate the connection string needed to connect to the db with authentication
    // Put this in for project on laptop
    // public string ConnectionString => $"mongodb://{User}:{Password}@{Host}:{Port}"; // With Authentication
}
