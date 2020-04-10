namespace ContractsService.v1.UserContracts.Responses
{
    public class ProfileResponse : IResponse
    {
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public UserResponse User { get; set; }
    }
}