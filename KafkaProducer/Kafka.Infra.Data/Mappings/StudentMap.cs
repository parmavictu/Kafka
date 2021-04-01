using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Kafka.Domain.Core;
using Kafka.Domain.Entities;

namespace Kafka.Infra.Data.Mappings
{
    public static class StudentMapping
    {
        public static void DefineClassMap()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Student)))
            {
                BsonClassMap.RegisterClassMap<Entity<Student>>(cm =>{
                    //cm.MapMember(s => s.Id);
                    cm.MapIdProperty(s => s.Id)
                     .SetIdGenerator(StringObjectIdGenerator.Instance)
                     .SetSerializer(new StringSerializer(BsonType.ObjectId));
                });
                BsonClassMap.RegisterClassMap<Student>(cm =>
                {
                    cm.SetIgnoreExtraElements(true);
                    cm.SetDiscriminator("Entity");
                    //cm.SetDiscriminator("AbstractValidator");
                    cm.AutoMap();
                    
                    cm.MapMember(s => s.Name).SetIsRequired(true);

                });
            }
        }
    }
}
