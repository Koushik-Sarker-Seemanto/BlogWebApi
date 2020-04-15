using System.Collections.Generic;
using System.Threading.Tasks;
using ContractsService.v1.PostContracts.Requests;
using ContractsService.v1.PostContracts.Response;

namespace PostHandlerService
{
    public interface IPostHandler
    {
        Task<GetAllPostResponse> GetAllPosts(string context);
        Task<GetAllPostResponse> UserAllPosts(string context);
        Task<GetPostByIdResponse> GetPostById(string id);
        Task<InsertPostResponse> InsertPost(InsertPostRequest request, string context);
        Task<UpdatePostResponse> UpdatePost(UpdatePostRequest request, string postId, string context);
        Task<DeletePostResponse> DeletePost(string postId, string context);
        Task<ReactResponse> AddReact(string context, string postId);
        Task<ReactResponse> ReactByUser(string context, string postId);
    }
}