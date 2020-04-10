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
            List<PostResponse> postResponses = new List<PostResponse>();
            foreach (var post in allPosts)
            {
                PostResponse tempPost = new PostResponse()
                {
                    Id = post.Id, Title = post.Title, Body = post.Body, Author = post.Author,
                    Likes = post.Likes, CreatedAt = post.CreatedAt, UpdatedAt = post.UpdatedAt, StatusCode = StatusCode.Ok,
                };
                postResponses.Add(tempPost);
            }
            
            return new GetAllPostResponse()
            {
                StatusCode = StatusCode.Ok,
                PostList = postResponses,
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

        public async Task<UpdatePostResponse> UpdatePost(UpdatePostRequest request, string postId, string context)
        {
            var user = await _userManager.GetUser(context);

            if (!request.IsValid())
            {
                return new UpdatePostResponse()
                {
                    StatusCode = StatusCode.InvalidArgument, ErrorMessage = "Empty argument provided",
                };
            }

            var post = await _postManager.GetPostById(postId);
            if (post == null)
            {
                return new UpdatePostResponse()
                {
                    StatusCode = StatusCode.NotFound, ErrorMessage = "Post not found",
                };
            }

            if (post.Author.Id != user.Id)
            {
                return new UpdatePostResponse()
                {
                    StatusCode = StatusCode.PermissionDenied, ErrorMessage = "Unauthorized user to update post",
                };
            }
            Post updatedPost = new Post()
            {
                Id = post.Id, Title = request.Title, Body = request.Body, Author = post.Author,
                CreatedAt = post.CreatedAt, UpdatedAt = DateTime.Now, Likes = post.Likes,
            };
            
            var updated = await _postManager.UpdatePost(updatedPost);
            if (!updated)
            {
                return new UpdatePostResponse()
                {
                    StatusCode = StatusCode.Internal, ErrorMessage = "Internal Error! Couldn't update post",
                };
            }
            return new UpdatePostResponse()
            {
                StatusCode = StatusCode.Ok,
                Post = new PostResponse()
                {
                    Id = updatedPost.Id, Title = updatedPost.Title, Body = updatedPost.Body, Author = updatedPost.Author,
                    CreatedAt = updatedPost.CreatedAt, UpdatedAt = updatedPost.UpdatedAt, Likes = updatedPost.Likes,
                }
            };
        }
    }
}