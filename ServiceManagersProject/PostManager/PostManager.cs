using System.Collections.Generic;
using ModelsProject;
using MongoDB.Driver;
using ModelsProject.Models;
using MongoDB.Bson;

namespace ServiceManagersProject
{
    public class PostManager : IPostManager
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private IMongoCollection<Post> _collection;
        public PostManager(IDatabaseSettings settings)
        {
            _mongoClient = new MongoClient(settings.ConnectionString);
            _mongoDatabase = _mongoClient.GetDatabase(settings.DatabaseName);
            _collection = _mongoDatabase.GetCollection<Post>("Posts");
        }

        public void DeletePost(string id)
        {
            var filter = Builders<Post>.Filter.Eq("Id", id);
            var record = _collection.FindOneAndDelete(filter);
        }

        public Post GetPost(string id)
        {
            var filter = Builders<Post>.Filter.Eq("Id", id);
            return  _collection.Find(filter).FirstOrDefault();
        }

        public List<Post> GetPostList()
        {
            var allPosts = _collection.Find(new BsonDocument());
            return allPosts.ToList();
        }

        public void InsertPost(Post post)
        {
            _collection.InsertOne(post);
        }

        public void UpdatePost(string id, Post post)
        {
            var filter = Builders<Post>.Filter.Eq("Id", id);
            var updated = _collection.ReplaceOne(filter, post);
        }
    }
}