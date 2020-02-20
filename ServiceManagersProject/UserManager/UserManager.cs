using System;
using System.Collections.Generic;
using ModelsProject;
using ModelsProject.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace ServiceManagersProject
{
    public class UserManager : IUserManager
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private IMongoCollection<User> _collection;
        public UserManager(IDatabaseSettings settings)
        {
            _mongoClient = new MongoClient(settings.ConnectionString);
            _mongoDatabase = _mongoClient.GetDatabase(settings.DatabaseName);
            _collection = _mongoDatabase.GetCollection<User>("Users");
        }
        

        public async Task<bool> DeleteUser(string id)
        {
            var filter = Builders<User>.Filter.Eq("Id", id);
            var record = await _collection.FindOneAndDeleteAsync(filter);
            if(record.Equals(null))
            {
                return false;
            }
            else
                return true;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq("Email", email);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<User> GetUser(string id)
        {
            var filter = Builders<User>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUserList()
        {
            var allusers = await _collection.FindAsync(new BsonDocument());
            return allusers.ToList();
        }

        public async Task<bool> InsertUser(User user)
        {
             await _collection.InsertOneAsync(user);
            return true;
        }

        public async Task<bool> UpdateUser(string id, User user)
        {
            var filter = Builders<User>.Filter.Eq("Id", id);
            var updated = await _collection.ReplaceOneAsync(filter, user);
            if(updated.Equals(null))
            {
                return false;
            }
            else
                return true;
        }
    }
}
