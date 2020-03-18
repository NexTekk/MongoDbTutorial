using System;
using System.Security.Authentication;
using MongoDB.Bson;
using MongoDB.Driver;
using Tutorial.Data.Documents;

namespace Tutorial.Data
{
    public class DbContext
    {
        private readonly Lazy<IMongoDatabase> _database;
        private readonly IDbSettings _appSettings;

        public DbContext(IDbSettings appSettings)
        {
            _appSettings = appSettings;
            _database = new Lazy<IMongoDatabase>(GetDatabase, true);
        }

        public IMongoCollection<UserDocument> Users => GetCollection<UserDocument>("users");

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
            var connectionUrl = new MongoUrl(_appSettings.DbServer);
            var settings = MongoClientSettings.FromUrl(connectionUrl);

            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = SslProtocols.Tls12,
            };

            var client = new MongoClient(settings);

            return client.GetDatabase(_appSettings.DbName);
        }
    }
}
