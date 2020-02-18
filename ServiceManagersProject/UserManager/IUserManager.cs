using System;
using System.Collections.Generic;
using ModelsProject;

namespace ServiceManagersProject
{
    public interface IUserManager
    {
        User GetUserByEmail(string email);
        List<User> GetUserList();
        bool InsertUser(User user);
        void UpdateUser(string id, User user);
        void DeleteUser(string id);
    }
}
