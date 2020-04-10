using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelsService.Configuration;
using ModelsService.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ModelsService.Managers.PostManager
{
    public class PostManager: IPostManager
    {
        private readonly IMongoCollection<Post> _collection;
        public PostManager(IDatabaseSettings settings)
        {
            IMongoClient mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);
            _collection = mongoDatabase.GetCollection<Post>("Posts");
        }
        
        /// <summary>
        /// This method returns all posts.
        /// </summary>
        /// <returns>Post list.</returns>
        public async Task<List<Post>> GetAllPost()
        {
            var allPosts = await _collection.FindAsync(new BsonDocument());
            return allPosts.ToList();
        }

        /// <summary>
        /// This method gets single post by id.
        /// </summary>
        /// <param name="id">PostId.</param>
        /// <returns>Post.</returns>
        public async Task<Post> GetPostById(string id)
        {
            var filter = Builders<Post>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// This method inserts post.
        /// </summary>
        /// <param name="post">Post.</param>
        /// <returns>boolean response.</returns>
        public async Task<bool> InsertPost(Post post)
        {
            await _collection.InsertOneAsync(post);
            return true;
        }

        /// <summary>
        /// This method updates post.
        /// </summary>
        /// <param name="post">Updated Post</param>
        /// <returns>boolean response.</returns>
        public async Task<bool> UpdatePost(Post post)
        {
            var filter = Builders<Post>.Filter.Eq("Id", post.Id);
            var updated = await _collection.ReplaceOneAsync(filter, post);
            return !updated.Equals(null);
        }

        /// <summary>
        /// This method deletes post.
        /// </summary>
        /// <param name="postId">PostId</param>
        /// <returns>boolean response.</returns>
        public async Task<bool> DeletePost(string postId)
        {
            var filter = Builders<Post>.Filter.Eq("Id", postId);

            var record = await _collection.FindOneAndDeleteAsync(filter);
            return !record.Equals(null);
        }

        /// <summary>
        /// This method gives like if the post is not liked previously.
        /// gives unlike if the post was liked before.
        /// </summary>
        /// <param name="id">PostId.</param>
        /// <param name="user">User.</param>
        /// <returns>boolean response.</returns>
        public async Task<bool> AddReact(string id, User user)
        {
            var filter = Builders<Post>.Filter.Eq("Id", id);
            var post = await _collection.Find(filter).FirstOrDefaultAsync();
            
            var likedUser = post.Likes.FirstOrDefault(e => e.Id == user.Id);
            if (likedUser == null)
            {
                post.Likes.Add(user);
                await UpdatePost(post);
                return true;
            }

            post.Likes.Remove(likedUser);
            var updated = await UpdatePost(post);
            return false;
        }
    }
}