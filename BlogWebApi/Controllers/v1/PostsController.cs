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
        public IActionResult InsertPost([FromBody]Post post)
        {
            if (post == null)
                return BadRequest();
            
            _postmanager.InsertPost(post);
            return Ok(post);
        }

        [HttpPut(ApiRoutes.PostRoute.UpdatePost)]
        public IActionResult UpdatePost(string id,[FromBody]Post newPost)
        {
            Post post = _postmanager.GetPost(id);
            if (post != null)
            {
                _postmanager.UpdatePost(id,newPost);
            }

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
