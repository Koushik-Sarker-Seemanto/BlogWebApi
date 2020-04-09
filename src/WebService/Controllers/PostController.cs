using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractsService.v1.PostContracts.Requests;
using ContractsService.v1.PostContracts.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelsService.Managers.PostManager;
using ModelsService.Managers.UserManager;
using ModelsService.Models;
using WebService.Routes.v1;

namespace WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private IPostManager _postManager;
        private IUserManager _userManager;
        public PostController(IPostManager postManager, IUserManager userManager)
        {
            _postManager = postManager;
            _userManager = userManager;
        }

        [HttpGet(ApiRoutes.PostRoute.GetPostList)]
        public async Task<ActionResult<List<Post>>> GetAllPosts()
        {
            var context = HttpContext.User.Identity.Name;

            var user = await _userManager.GetUser(context);
            if (user == null)
            {
                return BadRequest("Unauthorised user");
            }
            var allPosts = await _postManager.GetAllPost();
            return allPosts;
        }

        [HttpPost(ApiRoutes.PostRoute.InsertPost)]
        public async Task<ActionResult<Post>> InsertPost([FromBody]InsertPostRequest formData )
        {
            var context = HttpContext.User.Identity.Name;

            var user = await _userManager.GetUser(context);
            if (user == null)
            {
                return BadRequest("Unauthorised user");
            }
            
            Post post = new Post()
            {
                Title = formData.Title,
                Body = formData.Body,
                Author = user,
                Likes = null,
                UpdatedAt = null,
            };

            var result = await _postManager.InsertPost(post);
            if (result == false)
            {
                return BadRequest("Couldn't insert post");
            }

            return post;
        }
    }
}