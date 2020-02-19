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


        [AllowAnonymous]
        [HttpPost(ApiRoutes.UserRoute.Login)]
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
            return user.GetToken();
        }



        [AllowAnonymous]
        [HttpPost(ApiRoutes.UserRoute.Registration)]
        public ActionResult<string> Registration([FromBody]RegisterUserRequest formData)
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
                return newUser.GetToken();
            }
            return newUser.GetToken();
        }


        [HttpGet(ApiRoutes.UserRoute.Profile)]
        public ActionResult<UserProfile> Profile()
        {
            var user =  _userManager.GetUser(HttpContext.User.Identity.Name);
            if(user == null)
            {
                return NotFound("user not found");
            }
            return new UserProfile(user);
        }

    }
}
