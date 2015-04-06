using Api.App.Entities;

namespace Api.App.Images.Entities
{
    public class Genre : Entity
    {
        public const string CollectionName = "photoGenres";

        public string Name { get; set; }
    }
}