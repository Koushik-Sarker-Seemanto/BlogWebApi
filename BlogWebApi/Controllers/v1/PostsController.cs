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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BlogWebApi.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<IActionResult> GetPostList()
        {
            List<Post> tempPost = await _postmanager.GetPostList();
            return Ok(tempPost);
        }
        
        [HttpGet(ApiRoutes.PostRoute.GetPost)]
        public async Task<IActionResult> GetPost(string id)
        {
            Post post = await _postmanager.GetPost(id);
            return Ok(post);
        }
        
        [HttpPost(ApiRoutes.PostRoute.InsertPost)]
        public async Task<IActionResult> InsertPost([FromBody]InsertPostRequest requestPost)
        {
            if (requestPost.Title == "" || requestPost.Title == null)
                return BadRequest();
            
            Post post = new Post()
            {
                Title = requestPost.Title,
                Body = requestPost.Body
            };

            await _postmanager.InsertPost(post);

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
        public async Task<IActionResult> UpdatePost(string id,[FromBody]UpdatePostRequest newPost)
        {
            Post post = await _postmanager.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }
            post.Title = newPost.Title;
            post.Body = newPost.Body;
            post.updatedAt = newPost.updatedAt;

            var check = await _postmanager.UpdatePost(id, post);
            if(check)
                return Ok(newPost);

            return BadRequest();
        }
    
        [HttpDelete(ApiRoutes.PostRoute.DeletePost)]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            Post post = await _postmanager.GetPost(id);
            if (post != null)
            {
                var check = await _postmanager.DeletePost(id);
                if(!check)
                    return NotFound();
            }
            return Ok(post);
        }


    }
}
