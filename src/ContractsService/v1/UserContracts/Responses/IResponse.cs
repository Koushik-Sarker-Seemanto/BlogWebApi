namespace ContractsService.v1.UserContracts.Responses
{
    public interface IResponse
    {
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}