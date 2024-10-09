using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogSiteApplication.Models
{
    public class Blog
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("blogName")]
        public string? BlogName { get; set; }

        [BsonElement("category")]
        public string? Category { get; set;}

        [BsonElement("article")]
        public string? Article { get; set;}

        [BsonElement("authorId")]
        public string? AuthorId { get; set;}

        [BsonElement("authorName")]
        public string? AuthorName { get; set;}

        [BsonElement("timestamp")]
        public DateTime TimeStamp { get; set;}
    }
}
