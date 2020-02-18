using System;
using System.Collections.Generic;
using ModelsProject;
using System.Threading.Tasks;

namespace ServiceManagersProject
{
    public interface IPostManager
    {
        Task<Post> GetPost(string id);
        Task<List<Post>> GetPostList();
        Task<bool> InsertPost(Post post);
        Task<bool> UpdatePost(string id, Post post);
        Task<bool> DeletePost(string id);
    }
}
