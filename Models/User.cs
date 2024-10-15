using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace SocialApp.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("interests")]
        public List<string> Interests { get; set; } = new List<string>();

        [BsonElement("friends")]
        public List<ObjectId> Friends { get; set; } = new List<ObjectId>();

        [BsonElement("followers")]
        public List<ObjectId> Followers { get; set; } = new List<ObjectId>();

        [BsonElement("following")]
        public List<ObjectId> Following { get; set; } = new List<ObjectId>();
    }
}

