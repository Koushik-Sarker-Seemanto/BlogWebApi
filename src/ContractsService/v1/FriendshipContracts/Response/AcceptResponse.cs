using ContractsService.v1.PostContracts.Response;

namespace ContractsService.v1.FriendshipContracts.Response
{
    public class AcceptResponse : IResponse
    {
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}