using MongoDB.Driver;

namespace Black.Database.Base.UnitOfWorkBase.Concrete;

public class MongoDbContext
{
    private MongoClient Client { get; }
    private IMongoDatabase Database { get; }
    protected string _connectionString = "mongodb+srv://admin:2149357@fatay.btwp0.mongodb.net/admin";

    public MongoDbContext()
    {
        Client = new MongoClient(_connectionString);
        Database = Client.GetDatabase(_connectionString);
    }

    public MongoClient GetMongoClient()
    {
        return Client;
    }

    public IMongoDatabase GetMongoDatabase()
    {
        return Database;
    }
}