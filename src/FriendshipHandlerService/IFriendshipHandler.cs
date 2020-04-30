using System.Threading.Tasks;
using ContractsService.v1.FriendshipContracts.Response;

namespace FriendshipHandlerService
{
    public interface IFriendshipHandler
    {
        Task<FriendListResponse> GetFriendList(string context);
        Task<FriendRequestListResponse> GetFriendRequestList(string context);
        Task<AddFriendResponse> AddFriend(string fromUser, string toUser);
        Task<AcceptResponse> AcceptFriendRequest(string fromUser, string toUser);
        Task<RejectResponse> RejectFriendRequest(string fromUser, string toUser);
    }
}