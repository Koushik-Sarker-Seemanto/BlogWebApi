using System.Collections.Generic;
using ModelsService.Models;

namespace ContractsService.v1.PostContracts.Response
{
    public class GetAllPostResponse: IResponse
    {
        public List<Post> PostList { get; set; }
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}