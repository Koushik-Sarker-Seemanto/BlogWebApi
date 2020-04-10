using System.Collections.Generic;
using System.Threading.Tasks;
using ModelsService.Models;

namespace ModelsService.Managers.PostManager
{
    public interface IPostManager
    {
        Task<List<Post>> GetAllPost();
        
        Task<Post> GetPostById(string id);
        
        Task<bool> InsertPost(Post post);
        
        Task<bool> UpdatePost(Post post);
        
        Task<bool> DeletePost(string postId);
    }
}