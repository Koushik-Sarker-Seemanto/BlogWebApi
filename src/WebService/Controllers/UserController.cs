using System.Threading.Tasks;
using AuthenticationService;
using ContractsService.v1.UserContracts.Requests;
using ContractsService.v1.UserContracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebService.Routes.v1;
using Microsoft.AspNetCore.Http;

namespace WebService.Controllers
{
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
            var response = await _userManager.LoginUser(formData);

            if (response == ContractsService.StatusCode.InvalidArgument.ToString())
            {
                return BadRequest("Invalid argument provided");
            }
            
            if (response == ContractsService.StatusCode.NotFound.ToString())
            {
                return BadRequest("Invalid Email");
            }

            if (response == ContractsService.StatusCode.InvalidArgument.ToString()+" password")
            {
                return BadRequest("Invalid password");
            }
            return response;
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.UserRoute.Registration)]
        public async Task<ActionResult<string>> Registration([FromBody]RegisterRequest formData)
        {
            var response = await _userManager.RegisterUser(formData);
            
            if (response == ContractsService.StatusCode.AlreadyExists.ToString())
            {
                return BadRequest("Email already exists");
            }

            if (response == ContractsService.StatusCode.InvalidArgument.ToString())
            {
                return BadRequest("Invalid argument provided");
            }

            if (response == ContractsService.StatusCode.Internal.ToString())
            {
                return BadRequest("Internal error occured. Couldn't insert user.");
            }
            return response;
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
    }
}