namespace ContractsService.v1.PostContracts.Response
{
    public class DeletePostResponse : IResponse
    {
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public PostResponse Post { get; set; }
    }
}