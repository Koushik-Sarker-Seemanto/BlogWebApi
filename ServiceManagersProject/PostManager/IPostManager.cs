using System;
using System.Collections.Generic;
using ModelsProject;

namespace ServiceManagersProject
{
    public interface IPostManager
    {
        User GetPost(string id);
        List<User> GetPostList();
        void InsertPost(User user);
        void UpdatePost(string id, UserManager user);
        void DeletePost(string id);
    }
}
