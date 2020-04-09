using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContractsService;
using ContractsService.v1.PostContracts.Requests;
using ContractsService.v1.PostContracts.Response;
using ModelsService.Managers.PostManager;
using ModelsService.Managers.UserManager;
using ModelsService.Models;

namespace PostHandlerService
{
    public class PostHandler : IPostHandler
    {
        private IPostManager _postManager;
        private IUserManager _userManager;
        public PostHandler(IPostManager postManager, IUserManager userManager)
        {
            _postManager = postManager;
            _userManager = userManager;
        }

        public async Task<GetAllPostResponse> GetAllPosts()
        {
            var allPosts = await _postManager.GetAllPost();
            
            return new GetAllPostResponse()
            {
                StatusCode = StatusCode.Ok,
                PostList = allPosts,
            };
        }

        public async Task<PostResponse> GetPostById(string id)
        {
            var post = await _postManager.GetPostById(id);
            if (post == null)
            {
                return  new PostResponse()
                {
                    StatusCode = StatusCode.NotFound,
                    ErrorMessage = "Post not found",
                };
            }
            return new PostResponse()
            {
                StatusCode = StatusCode.Ok,
                Id = post.Id, Title = post.Title, Body = post.Body, Author = post.Author,
                Likes = post.Likes, CreatedAt = post.CreatedAt, UpdatedAt = post.UpdatedAt,
            };
        }

        public async Task<InsertPostResponse> InsertPost(InsertPostRequest request, string context)
        {
            var user = await _userManager.GetUser(context);

            if (!request.IsValid())
            {
                return new InsertPostResponse()
                {
                    StatusCode = StatusCode.InvalidArgument, ErrorMessage = "Empty argument provided",
                };
            }
            
            if (user == null)
            {
                return new InsertPostResponse()
                {
                    StatusCode = StatusCode.Unauthenticated, ErrorMessage = "Unauthorised user",
                };
            }
            
            Post post = new Post()
            {
                Title = request.Title, Body = request.Body, Author = user, Likes = null, UpdatedAt = null,
            };

            var result = await _postManager.InsertPost(post);
            if (result == false)
            {
                return new InsertPostResponse()
                {
                    StatusCode = StatusCode.Internal, ErrorMessage = "Internal Error! Couldn't insert post"
                };
            }

            return new InsertPostResponse()
            {
                StatusCode = StatusCode.Ok, Title = post.Title, Body = post.Body, Author = post.Author,
            };
        }
    }
}