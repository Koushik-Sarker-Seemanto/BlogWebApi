using System.Collections.Generic;
using ContractsService.v1.UserContracts.Responses;
using IResponse = ContractsService.v1.PostContracts.Response.IResponse;

namespace ContractsService.v1.FriendshipContracts.Response
{
    public class FriendListResponse : IResponse
    {
        public List<UserResponse> FriendList { get; set; }
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}