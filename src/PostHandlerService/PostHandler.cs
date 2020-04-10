using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContractsService;
using ContractsService.v1.PostContracts.Requests;
using ContractsService.v1.PostContracts.Response;
using ContractsService.v1.UserContracts.Responses;
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
                PostResponse tempPost = ConvertToPostResponse(post);
                postResponses.Add(tempPost);
            }
            
            return new GetAllPostResponse()
            {
                StatusCode = StatusCode.Ok,
                PostList = postResponses,
            };
        }

        public async Task<GetPostByIdResponse> GetPostById(string id)
        {
            var post = await _postManager.GetPostById(id);
            if (post == null)
            {
                return  new GetPostByIdResponse()
                {
                    StatusCode = StatusCode.NotFound,
                    ErrorMessage = "Post not found",
                };
            }
            return new GetPostByIdResponse()
            {
                StatusCode = StatusCode.Ok,
                Post = ConvertToPostResponse(post),
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
                StatusCode = StatusCode.Ok,
                Post = ConvertToPostResponse(post),
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
                Post = ConvertToPostResponse(updatedPost),
            };
        }

        public async Task<DeletePostResponse> DeletePost(string postId, string context)
        {
            var user = await _userManager.GetUser(context);
            var post = await _postManager.GetPostById(postId);
            if (post == null)
            {
                return new DeletePostResponse()
                {
                    StatusCode = StatusCode.NotFound, ErrorMessage = "Post not found",
                };
            }

            if (user.Id != post.Author.Id)
            {
                return new DeletePostResponse()
                {
                    StatusCode = StatusCode.PermissionDenied, ErrorMessage = "Unauthorized user to delete post",
                };
            }

            var deleted = await _postManager.DeletePost(post.Id);
            if (!deleted)
            {
                return new DeletePostResponse()
                {
                    StatusCode = StatusCode.Internal, ErrorMessage = "Internal Error! Couldn't delete post",
                };
            }
            return new DeletePostResponse()
            {
                StatusCode = StatusCode.Ok,
                Post = ConvertToPostResponse(post),
            };
        }


        private PostResponse ConvertToPostResponse(Post post)
        {
            PostResponse postResponse = new PostResponse()
            {
                Id = post.Id, Title = post.Title, Body = post.Body, Author = ConvertToUserResponse(post.Author),
                Likes = ConvertToUserResponseList(post.Likes), CreatedAt = post.CreatedAt, UpdatedAt = post.UpdatedAt,
            };
            return postResponse;
        }
        
        private UserResponse ConvertToUserResponse(User user)
        {
            if (user == null)
            {
                return null;
            }
            UserResponse userResponse = new UserResponse()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
            };
            return userResponse;
        }
        private List<UserResponse> ConvertToUserResponseList(List<User> userList)
        {
            List<UserResponse> responseList = new List<UserResponse>();
            if (userList == null)
            {
                return null;
            }
            foreach (var user in userList)
            {
                UserResponse tempUser = new UserResponse()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                };
                responseList.Add(tempUser);
            }
            
            return responseList;
        }
    }
}