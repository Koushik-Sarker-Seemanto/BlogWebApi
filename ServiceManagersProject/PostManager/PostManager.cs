using System.Collections.Generic;
using ModelsProject;
using MongoDB.Driver;
using ModelsProject.Models;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

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

        public async Task<bool> DeletePost(string id)
        {
            var filter = Builders<Post>.Filter.Eq("Id", id);
            var record = await _collection.FindOneAndDeleteAsync(filter);
            if(record.Equals(null))
            {
                return false;
            }
            else
                return true;
        }

        public async Task<Post> GetPost(string id)
        {
            var filter = Builders<Post>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Post>> GetPostList()
        {
            var allPosts = await _collection.FindAsync(new BsonDocument());
            return allPosts.ToList();
        }

        public async Task<bool> InsertPost(Post post)
        {
            await _collection.InsertOneAsync(post);
            return true;
        }

        public async Task<bool> UpdatePost(string id, Post post)
        {
            var filter = Builders<Post>.Filter.Eq("Id", id);
            var updated = await _collection.ReplaceOneAsync(filter, post);
            if(updated.Equals(null))
            {
                return false;
            }
            else
                return true;
        }
    }
}