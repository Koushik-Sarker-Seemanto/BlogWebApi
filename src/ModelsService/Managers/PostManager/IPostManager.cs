using System.Collections.Generic;
using System.Threading.Tasks;
using ModelsService.Models;

namespace ModelsService.Managers.PostManager
{
    public interface IPostManager
    {
        Task<List<Post>> GetAllPost();
        Task<List<Post>> GetAllPost(string userId);
        
        Task<Post> GetPostById(string id);
        
        Task<bool> InsertPost(Post post);
        
        Task<bool> UpdatePost(Post post);
        
        Task<bool> DeletePost(string postId);
        Task<bool> AddReact(string id, User user);
    }
}