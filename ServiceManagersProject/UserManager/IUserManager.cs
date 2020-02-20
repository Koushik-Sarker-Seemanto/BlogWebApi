using System;
using System.Collections.Generic;
using ModelsProject;
using System.Threading.Tasks;

namespace ServiceManagersProject
{
    public interface IUserManager
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUser(string id);
        Task<List<User>> GetUserList();
        Task<bool> InsertUser(User user);
        Task<bool> UpdateUser(string id, User user);
        Task<bool> DeleteUser(string id);
    }
}
