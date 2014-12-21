using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Entities
{
    public abstract class Entity : IEntity
    {
        protected Entity()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }
    }
}