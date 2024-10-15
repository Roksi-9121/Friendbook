using MongoDB.Driver;
using SocialApp.Models;

namespace SocialApp.Services
{
    public class DatabaseService
    {
        private readonly IMongoDatabase _database;

        public DatabaseService()
        {
            var client = new MongoClient("mongodb+srv://roksolana9121:wfnodjMcKda7RbEe@cluster0.7uklx.mongodb.net/social_network?retryWrites=true&w=majority\r\n");
            _database = client.GetDatabase("social_network");
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
        public IMongoCollection<Post> Posts => _database.GetCollection<Post>("posts");
        public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>("comments");

    }
}
