using System.Collections.Generic;
using System.Threading.Tasks;
using ModelsService.Configuration;
using ModelsService.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ModelsService.Managers.UserManager
{
    public class UserManager : IUserManager
    {
        private readonly IMongoCollection<User> _collection;
        public UserManager(IDatabaseSettings settings)
        {
            IMongoClient mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);
            _collection = mongoDatabase.GetCollection<User>("Users");
        }
        
        /// <summary>
        /// This method deletes user.
        /// </summary>
        /// <param name="id">UserId.</param>
        /// <returns>boolean response.</returns>
        public async Task<bool> DeleteUser(string id)
        {
            var filter = Builders<User>.Filter.Eq("Id", id);
            var record = await _collection.FindOneAndDeleteAsync(filter);
            return !record.Equals(null);
        }

        /// <summary>
        /// This method gets single user by email.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>User.</returns>
        public async Task<User> GetUserByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq("Email", email);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// This method gets single user by Id.
        /// </summary>
        /// <param name="id">UserId.</param>
        /// <returns></returns>
        public async Task<User> GetUser(string id)
        {
            var filter = Builders<User>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// This method gets all the users.
        /// </summary>
        /// <returns>User list.</returns>
        public async Task<List<User>> GetUserList()
        {
            var allUsers = await _collection.FindAsync(new BsonDocument());
            return allUsers.ToList();
        }

        /// <summary>
        /// This method inserts user.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>boolean response.</returns>
        public async Task<bool> InsertUser(User user)
        {
            await _collection.InsertOneAsync(user);
            return true;
        }

        /// <summary>
        /// This method updates user.
        /// </summary>
        /// <param name="id">UserId.</param>
        /// <param name="user">User.</param>
        /// <returns>boolean response.</returns>
        public async Task<bool> UpdateUser(string id, User user)
        {
            var filter = Builders<User>.Filter.Eq("Id", id);
            var updated = await _collection.ReplaceOneAsync(filter, user);
            return !updated.Equals(null);
        }
    }
}