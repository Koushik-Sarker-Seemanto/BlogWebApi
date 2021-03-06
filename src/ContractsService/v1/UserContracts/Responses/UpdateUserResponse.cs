namespace ContractsService.v1.UserContracts.Responses
{
    public class UpdateUserResponse : IResponse
    {
        public string Name{ get; set; }
        public string Email { get; set; }
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}