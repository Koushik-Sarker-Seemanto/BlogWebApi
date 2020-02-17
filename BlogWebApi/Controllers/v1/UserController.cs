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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet(ApiRoutes.UserRoute.GetAllusers)]
        public string Get()
        {
            var name ="koushik";
            return name;
        }
    }
}
