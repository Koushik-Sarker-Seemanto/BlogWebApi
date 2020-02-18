using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWebApi.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using ServiceManagersProject;
using ModelsProject;
using BlogWebApi.Contracts.v1.Requests;
using BlogWebApi.Contracts.v1.Responses;

namespace BlogWebApi.Controllers.v1
{
    [ApiController]
    // [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostManager _postmanager;
        public PostsController(IPostManager postManager)
        {
            _postmanager = postManager;
        }

        [HttpGet(ApiRoutes.PostRoute.GetPostList)]
        public IActionResult GetPostList()
        {
            List<Post> tempPost = _postmanager.GetPostList();
            return Ok(tempPost);
        }
        
        [HttpGet(ApiRoutes.PostRoute.GetPost)]
        public IActionResult GetPost(string id)
        {
            Post post = _postmanager.GetPost(id);
            return Ok(post);
        }
        
        [HttpPost(ApiRoutes.PostRoute.InsertPost)]
        public IActionResult InsertPost([FromBody]InsertPostRequest requestPost)
        {
            if (requestPost.Title == "" || requestPost.Title == null)
                return BadRequest();
            
            Post post = new Post()
            {
                Title = requestPost.Title,
                Body = requestPost.Body
            };

            _postmanager.InsertPost(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.PostRoute.GetPost.Replace("{id}",post.Id);

            var responsePost = new InsertPostRespnse()
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body
            };

            return Created(locationUrl, responsePost);
            //return Ok(post);
        }

        [HttpPut(ApiRoutes.PostRoute.UpdatePost)]
        public IActionResult UpdatePost(string id,[FromBody]UpdatePostRequest newPost)
        {
            Post post = _postmanager.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }
            post.Title = newPost.Title;
            post.Body = newPost.Body;
            post.updatedAt = newPost.updatedAt;

            _postmanager.UpdatePost(id, post);

            return Ok(newPost);
        }
    
        [HttpDelete(ApiRoutes.PostRoute.DeletePost)]
        public IActionResult DeleteStudent(string id)
        {
            Post post = _postmanager.GetPost(id);
            if (post != null)
            {
                _postmanager.DeletePost(id);
            }
            return Ok(post);
        }


    }
}
