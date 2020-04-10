namespace ContractsService.v1.PostContracts.Response
{
    public class UpdatePostResponse : IResponse
    {
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public PostResponse Post { get; set; }
    }
}