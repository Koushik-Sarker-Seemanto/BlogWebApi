using System.Collections.Generic;
using System.Threading.Tasks;
using ModelsService.Configuration;
using ModelsService.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ModelsService.Managers.PostManager
{
    public class PostManager: IPostManager
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
        
        public async Task<List<Post>> GetAllPost()
        {
            var allPosts = await _collection.FindAsync(new BsonDocument());
            return allPosts.ToList();
        }

        public async Task<Post> GetPostById(string id)
        {
            var filter = Builders<Post>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertPost(Post post)
        {
            await _collection.InsertOneAsync(post);
            return true;
        }

        public async Task<bool> UpdatePost(Post post)
        {
            var filter = Builders<Post>.Filter.Eq("Id", post.Id);
            var updated = await _collection.ReplaceOneAsync(filter, post);
            if(updated.Equals(null))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeletePost(string postId)
        {
            var filter = Builders<Post>.Filter.Eq("Id", postId);

            var record = await _collection.FindOneAndDeleteAsync(filter);
            return !record.Equals(null);
        }
    }
}