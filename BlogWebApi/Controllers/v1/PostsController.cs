using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWebApi.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace BlogWebApi.Controllers.v1
{
    [ApiController]
    // [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        public PostsController()
        {

        }

        [HttpGet(ApiRoutes.PostRoute.GetAllPosts)]
        public string GetAllPosts()
        {
            return "posts";
        }
    }
}
