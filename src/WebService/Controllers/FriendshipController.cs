using System.Threading.Tasks;
using ContractsService.v1.FriendshipContracts.Response;
using FriendshipHandlerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers
{
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipHandler _friendshipHandler;
        public FriendshipController(IFriendshipHandler friendshipHandler)
        {
            _friendshipHandler = friendshipHandler;
        }

        [HttpGet(Routes.v1.ApiRoutes.FriendshipRoute.FriendList)]
        public async Task<ActionResult<FriendListResponse>> FriendList()
        {
            var context = HttpContext.User.Identity.Name;
            var result = await this._friendshipHandler.GetFriendList(context);
            switch (result.StatusCode)
            {
                case ContractsService.StatusCode.NotFound:
                    return BadRequest(result.ErrorMessage);
                case ContractsService.StatusCode.Ok:
                    return result;
                default:
                    return BadRequest("Unknown error");
            }
        }

        [HttpGet(Routes.v1.ApiRoutes.FriendshipRoute.FriendRequestList)]
        public async Task<ActionResult<FriendRequestListResponse>> FriendRequestList()
        {
            var context = HttpContext.User.Identity.Name;
            var result = await this._friendshipHandler.GetFriendRequestList(context);
            if (result.StatusCode != ContractsService.StatusCode.Ok)
            {
                return BadRequest(result.ErrorMessage);
            }

            return result;
        }

        [HttpGet(Routes.v1.ApiRoutes.FriendshipRoute.AddFriend)]
        public async Task<ActionResult<AddFriendResponse>> AddFriend(string id)
        {
            var fromUser = HttpContext.User.Identity.Name;
            var result = await this._friendshipHandler.AddFriend(fromUser, id);

            if (result.StatusCode != ContractsService.StatusCode.Ok)
            {
                return BadRequest(result.ErrorMessage);
            }

            return result;
        }

        [HttpGet(Routes.v1.ApiRoutes.FriendshipRoute.AcceptRequest)]
        public async Task<ActionResult<AcceptResponse>> AcceptFriendRequest(string id)
        {
            var fromUser = HttpContext.User.Identity.Name;
            var result = await this._friendshipHandler.AcceptFriendRequest(fromUser, id);
            
            if (result.StatusCode != ContractsService.StatusCode.Ok)
            {
                return BadRequest(result.ErrorMessage);
            }

            return result;
        }
        
        [HttpGet(Routes.v1.ApiRoutes.FriendshipRoute.RejectRequest)]
        public async Task<ActionResult<RejectResponse>> RejectFriendRequest(string id)
        {
            var fromUser = HttpContext.User.Identity.Name;
            var result = await this._friendshipHandler.RejectFriendRequest(fromUser, id);
            
            if (result.StatusCode != ContractsService.StatusCode.Ok)
            {
                return BadRequest(result.ErrorMessage);
            }

            return result;
        }
    }
}