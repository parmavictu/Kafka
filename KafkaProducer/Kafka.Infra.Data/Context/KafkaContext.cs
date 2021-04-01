using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Kafka.Infra.Data.Mappings;
using System;
using System.Linq;

namespace Kafka.Infra.Data.Context
{
    public class KafkaContext<TEntity> where TEntity : class
    {
        private IMongoDatabase _database { get; }
        public KafkaContext()
        {
            try
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(Environment.GetEnvironmentVariable("CONNECTION_STRING")));
                var mongoClient = new MongoClient(settings);
                _database = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("CONNECTION_STRING").Split('/').LastOrDefault().Trim());

                #region  RegisterMappings
                StudentMapping.DefineClassMap();
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível se conectar com o servidor.", ex);
            }
        }
        public IMongoCollection<TEntity> Collection
        {
            get
            {
                string collectionName = typeof(TEntity).Name.ToLowerInvariant()[0] + typeof(TEntity).Name.Substring(1);
                IMongoCollection<TEntity> collection = _database.GetCollection<TEntity>(collectionName);

                if (collection == null)
                {
                    _database.CreateCollection(collectionName);
                }
                return collection;
            }
        }
    }
}
