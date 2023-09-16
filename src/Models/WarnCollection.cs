using MongoDB.Bson;

namespace Sushi.Models
{
    public class WarnCollection
    {
        public class MaxWarnsObject
        {
            public ObjectId Id { get; set; }
            public string? GuildId { get; set; }
            public int MaxWarns { get; set; }
        }
    }
}
