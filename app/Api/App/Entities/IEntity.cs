using MongoDB.Bson;

namespace Api.App.Entities
{
    public interface IEntity
    {
        ObjectId _id { get; set; }        
    }
}