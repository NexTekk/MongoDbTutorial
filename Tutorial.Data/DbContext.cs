using System;
using System.Security.Authentication;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Tutorial.Data
{
    public class DbContext
    {
        private const string UsersCollectionName = "users";

        private readonly Lazy<IMongoDatabase> _database;
        private readonly IDbSettings _dbSettings;

        public DbContext(IDbSettings dbSettings)
        {
            _dbSettings = dbSettings;
            _database = new Lazy<IMongoDatabase>(GetDatabase, true);
        }

        public IMongoCollection<UserDocument> Users 
            => GetCollection<UserDocument>(UsersCollectionName);

        private IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            var collectionSettings = new MongoCollectionSettings
            {
                GuidRepresentation = GuidRepresentation.Standard
            };

            return _database.Value.GetCollection<T>(collectionName, collectionSettings);
        }

        private IMongoDatabase GetDatabase()
        {
            var connectionUrl = new MongoUrl(_dbSettings.DbServer);
            var settings = MongoClientSettings.FromUrl(connectionUrl);

            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = SslProtocols.Tls12,
            };

            var client = new MongoClient(settings);

            return client.GetDatabase(_dbSettings.DbName);
        }
    }
}
