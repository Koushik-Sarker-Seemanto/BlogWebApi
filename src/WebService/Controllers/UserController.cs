using System.Threading.Tasks;
using AuthenticationService;
using ContractsService.v1.UserContracts.Requests;
using ContractsService.v1.UserContracts.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebService.Routes.v1;
using Microsoft.AspNetCore.Http;

namespace WebService.Controllers
{
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserAuthenticator _userManager;
        public UserController(IUserAuthenticator userManager)
        {
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.UserRoute.Login)]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest formData)
        {
            var result = await _userManager.LoginUser(formData);
            var response = result.StatusCode;

            if (response == ContractsService.StatusCode.InvalidArgument)
            {
                return BadRequest(result.ErrorMessage);
            }
            
            if (response == ContractsService.StatusCode.NotFound)
            {
                return BadRequest(result.ErrorMessage);
            }
            return result.Token;
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.UserRoute.Registration)]
        public async Task<ActionResult<string>> Registration([FromBody]RegisterRequest formData)
        {
            var result = await _userManager.RegisterUser(formData);
            var response = result.StatusCode;
            
            if (response == ContractsService.StatusCode.AlreadyExists)
            {
                return BadRequest(result.ErrorMessage);
            }

            if (response == ContractsService.StatusCode.InvalidArgument)
            {
                return BadRequest(result.ErrorMessage);
            }

            if (response == ContractsService.StatusCode.Internal)
            {
                return BadRequest(result.ErrorMessage);
            }
            return result.Token;
        }

        [HttpGet(ApiRoutes.UserRoute.Profile)]
        public async Task<ActionResult<ProfileResponse>> Profile()
        {
            var context = HttpContext.User.Identity.Name;
            var user = await _userManager.ReturnProfile(context);
            if (user == null)
            {
                return BadRequest(StatusCodes.Status404NotFound);
            }
            return user;
        }
        
        [HttpPost(ApiRoutes.UserRoute.UpdateProfile)]
        public async Task<ActionResult<UpdateUserResponse>> Update([FromBody]UpdateUserRequest formData)
        {
            var context = HttpContext.User.Identity.Name;
            var result = await _userManager.UpdateUser(formData, context);
            
            var response = result.StatusCode;

            if (response == ContractsService.StatusCode.Unauthenticated)
            {
                return BadRequest(result.ErrorMessage);
            }
            
            if (response == ContractsService.StatusCode.AlreadyExists)
            {
                return BadRequest(result.ErrorMessage);
            }

            if (response == ContractsService.StatusCode.InvalidArgument)
            {
                return BadRequest(result.ErrorMessage);
            }

            if (response == ContractsService.StatusCode.Internal)
            {
                return BadRequest(result.ErrorMessage);
            }
            return result;
        }

    }
}