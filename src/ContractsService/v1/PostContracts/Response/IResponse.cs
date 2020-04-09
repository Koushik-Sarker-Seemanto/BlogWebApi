namespace ContractsService.v1.PostContracts.Response
{
    public interface IResponse
    {
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}