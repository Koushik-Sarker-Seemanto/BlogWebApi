using System;
using System.Collections.Generic;
using ModelsProject;
using ModelsProject.Models;

namespace ServiceManagersProject
{
    public class UserManager : IUserManager
    {
        public UserManager()
        {
            
        }

        public void DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public List<User> GetUserList()
        {
            throw new NotImplementedException();
        }

        public bool InsertUser(User user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(string id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
