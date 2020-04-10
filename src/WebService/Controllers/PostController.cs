using System.Threading.Tasks;
using ContractsService.v1.PostContracts.Requests;
using ContractsService.v1.PostContracts.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostHandlerService;
using WebService.Routes.v1;

namespace WebService.Controllers
{
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostHandler _postHandler;
        public PostController(IPostHandler postHandler)
        {
            _postHandler = postHandler;
        }

        [HttpGet(ApiRoutes.PostRoute.GetPostList)]
        public async Task<ActionResult<GetAllPostResponse>> GetAllPosts()
        {
            var result = await _postHandler.GetAllPosts();
            if (result.StatusCode == ContractsService.StatusCode.Ok)
            {
                return result;
            }
            return BadRequest("Unknown error");
        }

        [HttpGet(ApiRoutes.PostRoute.GetPost)]
        public async Task<ActionResult<GetPostByIdResponse>> GetPostById(string id)
        {
            var result = await _postHandler.GetPostById(id);
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

        [HttpPost(ApiRoutes.PostRoute.InsertPost)]
        public async Task<ActionResult<InsertPostResponse>> InsertPost([FromBody]InsertPostRequest formData )
        {
            var context = HttpContext.User.Identity.Name;

            var result = await _postHandler.InsertPost(formData, context);

            switch (result.StatusCode)
            {
                case ContractsService.StatusCode.Unauthenticated:
                    return BadRequest(result.ErrorMessage);
                case ContractsService.StatusCode.Internal:
                    return BadRequest(result.ErrorMessage);
                case ContractsService.StatusCode.InvalidArgument:
                    return BadRequest(result.ErrorMessage);
                case ContractsService.StatusCode.Ok:
                    return result;
                default:
                    return BadRequest("Unknown error");
            }
        }

        [HttpPut(ApiRoutes.PostRoute.UpdatePost)]
        public async Task<ActionResult<UpdatePostResponse>> UpdatePost([FromBody] UpdatePostRequest formData, string id)
        {
            var context = HttpContext.User.Identity.Name;
            var result = await _postHandler.UpdatePost(formData, id, context);

            switch (result.StatusCode)
            {
                case ContractsService.StatusCode.PermissionDenied:
                    return BadRequest(result.ErrorMessage);
                case ContractsService.StatusCode.NotFound:
                    return BadRequest(result.ErrorMessage);
                case ContractsService.StatusCode.InvalidArgument:
                    return BadRequest(result.ErrorMessage);
                case ContractsService.StatusCode.Internal:
                    return BadRequest(result.ErrorMessage);
                case ContractsService.StatusCode.Ok:
                    return result;
                default:
                    return BadRequest("Unknown error");
            }
        }

        [HttpDelete(ApiRoutes.PostRoute.DeletePost)]
        public async Task<ActionResult<DeletePostResponse>> DeletePost(string id)
        {
            var context = HttpContext.User.Identity.Name;
            var result = await _postHandler.DeletePost(id, context);

            switch (result.StatusCode)
            {
                case ContractsService.StatusCode.NotFound:
                    return BadRequest(result.ErrorMessage);
                case ContractsService.StatusCode.PermissionDenied:
                    return BadRequest(result.ErrorMessage);
                case ContractsService.StatusCode.Internal:
                    return BadRequest(result.ErrorMessage);
                case ContractsService.StatusCode.Ok:
                    return result;
                default:
                    return BadRequest("Unknown error");
            }
        }
    }
}