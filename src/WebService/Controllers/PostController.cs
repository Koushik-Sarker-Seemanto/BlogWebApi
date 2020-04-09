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
using PostHandlerService;
using WebService.Routes.v1;

namespace WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private IPostHandler _postHandler;
        public PostController(IPostHandler postHandler)
        {
            _postHandler = postHandler;
        }

        [HttpGet(ApiRoutes.PostRoute.GetPostList)]
        public async Task<ActionResult<GetAllPostResponse>> GetAllPosts()
        {
            var context = HttpContext.User.Identity.Name;

            var result = await _postHandler.GetAllPosts(context);
            if (result.StatusCode == ContractsService.StatusCode.Unauthenticated)
            {
                return BadRequest(result.ErrorMessage);
            }
            return result;
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
                default:
                    return result;
            }
        }
    }
}