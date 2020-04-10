namespace ContractsService.v1.PostContracts.Response
{
    public class ReactResponse : IResponse
    {
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string LikeOrUnlike { get; set; }
        public PostResponse Post { get; set; }
    }
}