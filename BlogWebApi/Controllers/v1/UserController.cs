using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWebApi.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ServiceManagersProject;
using ModelsProject;
using BlogWebApi.Contracts.v1.Requests.AuthRequest;
using BlogWebApi.Contracts.v1.Responses;
using BlogWebApi.Extend;

namespace BlogWebApi.Controllers.v1
{
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet(ApiRoutes.UserRoute.GetAllusers)]
        public string Get()
        {
            var name ="koushik";
            return name;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginUserRequest formData)
        {
            var user = _userManager.GetUserByEmail(formData.Email);
            if(user == null)
            {
                return BadRequest("no user found with this email");
            }
            if(user.CheckPassword(formData.Password) == false)
            {
                return BadRequest("incorrect password");
            }
            return "token";
            //return user.GetToken();
        }
        [AllowAnonymous]
        [HttpPost("register", Name="CreateUser")]
        public ActionResult<string> Create([FromBody]RegisterUserRequest formData)
        {
            var existingUser = _userManager.GetUserByEmail(formData.Email);
            if(existingUser != null)
            {
                return BadRequest("user with this email already registered");
            }
            var newUser = new User();
            newUser.Email = formData.Email;
            newUser.Name = formData.Name;
            newUser.SetPassword(formData.Password);
            var added = _userManager.InsertUser(newUser);
            if(added)
            {
                //return newUser.GetToken();
                return "token";
            }
            //return newUser.GetToken();
            return "token";
        }
    }
}
