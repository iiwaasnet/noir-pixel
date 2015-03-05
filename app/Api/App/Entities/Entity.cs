using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Entities
{
    public abstract class Entity : IEntity
    {
        public string InitEntityId()
        {
            Id = ObjectId.GenerateNewId().ToString();

            return Id;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}