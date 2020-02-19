using System;
using System.Collections.Generic;
using ModelsProject;
using ModelsProject.Models;
using MongoDB.Driver;
using MongoDB.Bson;

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
        

        public void DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq("Email", email);
            return _collection.Find(filter).FirstOrDefault();
        }

        public User GetUser(string id)
        {
            var filter = Builders<User>.Filter.Eq("Id", id);
            return _collection.Find(filter).FirstOrDefault();
        }

        public List<User> GetUserList()
        {
            throw new NotImplementedException();
        }

        public bool InsertUser(User user)
        {
            _collection.InsertOne(user);
            return true;
        }

        public void UpdateUser(string id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
