using System;
using System.Collections.Generic;
using ModelsProject;

namespace ServiceManagersProject
{
    public interface IUserManager
    {
        User GetUser(string id);
        List<User> GetUserList();
        void InsertUser(User user);
        void UpdateUser(string id, UserManager user);
        void DeleteUser(string id);
    }
}
