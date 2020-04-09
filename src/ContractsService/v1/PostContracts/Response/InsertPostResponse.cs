using ModelsService.Models;

namespace ContractsService.v1.PostContracts.Response
{
    public class InsertPostResponse : IResponse
    {
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public User Author { get; set; }
    }
}