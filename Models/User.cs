using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogSiteApplication.Models
{
    public class User
    {
        [BsonId]
        public ObjectId ObjectID { get; set; }

        [BsonElement("username")]
        public string? UserName { get; set; }

        [BsonElement("email")]
        public string? UserEmail { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("roles")]
        public string[]? Roles { get; set; }
    }
}
