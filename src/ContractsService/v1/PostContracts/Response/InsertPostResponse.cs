using ModelsService.Models;

namespace ContractsService.v1.PostContracts.Response
{
    public class InsertPostResponse : IResponse
    {
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public PostResponse Post { get; set; }
    }
}