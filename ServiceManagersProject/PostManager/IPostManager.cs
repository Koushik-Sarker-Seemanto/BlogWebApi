using System;
using System.Collections.Generic;
using ModelsProject;

namespace ServiceManagersProject
{
    public interface IPostManager
    {
        Post GetPost(string id);
        List<Post> GetPostList();
        void InsertPost(Post post);
        void UpdatePost(string id, Post post);
        void DeletePost(string id);
    }
}
