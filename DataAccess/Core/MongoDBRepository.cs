using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using Auctus.Util.NotShared;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Linq;
using Auctus.DomainObjects;

namespace Auctus.DataAccess.Core
{
    public class MongoDBRepository
    {
        private const string DATABASE_NAME = "AucutusPlatform";
        internal static IMongoDatabase GetDataBase()
        {
            MongoClient client = new MongoClient(Config.MONGO_CONNECTION_STRING);
            IMongoDatabase database = client.GetDatabase(DATABASE_NAME);
            return database;
        }
        protected Task InsertOneAsync(string collectionName, MongoDomainObject document)
        {
            IMongoDatabase database = GetDataBase();
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);
            return collection.InsertOneAsync(document.ToBsonDocument());
        }

        protected Task InsertManyAsync(string collectionName, IEnumerable<MongoDomainObject> documents)
        {
            IMongoDatabase database = GetDataBase();
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);
            return collection.InsertManyAsync(documents.Select(x => x.ToBsonDocument()));
        }

    }
}
