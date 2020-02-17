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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }
    }
}