using System.Collections.Generic;
using ContractsService.v1.UserContracts.Responses;
using IResponse = ContractsService.v1.PostContracts.Response.IResponse;

namespace ContractsService.v1.FriendshipContracts.Response
{
    public class FriendRequestListResponse: IResponse
    {
        public List<UserResponse> FriendRequestList { get; set; }
        public StatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}