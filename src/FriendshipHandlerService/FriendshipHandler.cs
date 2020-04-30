using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractsService;
using ContractsService.v1.FriendshipContracts.Response;
using ContractsService.v1.UserContracts.Responses;
using ModelsService.Managers.UserManager;
using ModelsService.Models;

namespace FriendshipHandlerService
{
    public class FriendshipHandler : IFriendshipHandler
    {
        private readonly IUserManager _userManager;
        public FriendshipHandler(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<FriendListResponse> GetFriendList(string context)
        {
            var user = await this._userManager.GetUser(context);
            if (user == null)
            {
                return new FriendListResponse {StatusCode = StatusCode.NotFound, ErrorMessage = "User not found"};
            }

            var list = user.Friends;
            var response = new FriendListResponse { StatusCode = StatusCode.Ok, FriendList = new List<UserResponse>()};
            foreach (var userId in list)
            {
                var tempUser = await this._userManager.GetUser(userId);
                if (tempUser != null)
                {
                    response.FriendList.Add(ConvertToUserResponse(tempUser));
                }
            }

            return response;
        }

        public async Task<FriendRequestListResponse> GetFriendRequestList(string context)
        {
            var user = await _userManager.GetUser(context);
            if (user == null)
            {
                return new FriendRequestListResponse {StatusCode = StatusCode.NotFound, ErrorMessage = "User not found"};
            }
            var list = user.FriendRequests;
            var response = new FriendRequestListResponse { StatusCode = StatusCode.Ok, FriendRequestList = new List<UserResponse>()};
            foreach (var userId in list)
            {
                var tempUser = await this._userManager.GetUser(userId);
                if (tempUser != null)
                {
                    response.FriendRequestList.Add(ConvertToUserResponse(tempUser));
                }
            }

            return response;
        }

        public async Task<AddFriendResponse> AddFriend(string fromUser, string toUser)
        {
            if (fromUser == toUser)
            {
                return new AddFriendResponse {StatusCode = StatusCode.InvalidArgument, ErrorMessage = "User can't sent request to himself/herself"};
            }
            var from = await this._userManager.GetUser(fromUser);
            var to = await this._userManager.GetUser(toUser);
            if (from == null || to == null)
            {
                return new AddFriendResponse {StatusCode = StatusCode.NotFound, ErrorMessage = "User not found",};
            }

            var fromRequestSent = from.FriendRequestsSent.FirstOrDefault(e => e == toUser);
            if (fromRequestSent != null)
            {
                return new AddFriendResponse {StatusCode = StatusCode.AlreadyExists, ErrorMessage = "Request already sent"};
            }
            var fromRequestList = from.FriendRequests.FirstOrDefault(e => e == toUser);
            if (fromRequestList != null)
            {
                return new AddFriendResponse {StatusCode = StatusCode.AlreadyExists, ErrorMessage = "User already requested you"};
            }
            var fromFriend = from.Friends.FirstOrDefault(e => e == toUser);
            if (fromFriend != null)
            {
                return new AddFriendResponse {StatusCode = StatusCode.AlreadyExists, ErrorMessage = "Already friend"};
            }

            from.FriendRequestsSent.Add(toUser);
            to.FriendRequests.Add(fromUser);
            var updateFrom = await _userManager.UpdateUser(from.Id, from);
            var updateTo = await _userManager.UpdateUser(to.Id, to);
            if (!updateFrom || !updateTo)
            {
                return new AddFriendResponse {StatusCode = StatusCode.Internal, ErrorMessage = "Internal error! Couldn't update users"};
            }

            return new AddFriendResponse
            {
                StatusCode = StatusCode.Ok,
                ErrorMessage = $"Request successfully sent From: {from.Name}, To: {to.Name}",
            };
        }

        public async Task<AcceptResponse> AcceptFriendRequest(string fromUser, string toUser)
        {
            if (fromUser == toUser)
            {
                return new AcceptResponse {StatusCode = StatusCode.InvalidArgument, ErrorMessage = "User can't accept request of himself/herself"};
            }
            var from = await this._userManager.GetUser(fromUser);
            var to = await this._userManager.GetUser(toUser);
            if (from == null || to == null)
            {
                return new AcceptResponse {StatusCode = StatusCode.NotFound, ErrorMessage = "User not found",};
            }

            var fromCheck = from.FriendRequests.FirstOrDefault(e => e == toUser);
            if (fromCheck == null)
            {
                return new AcceptResponse {StatusCode = StatusCode.NotFound, ErrorMessage = "Friend request not found",};
            }
            var toCheck = to.FriendRequestsSent.FirstOrDefault(e => e == fromUser);
            if (toCheck == null)
            {
                return new AcceptResponse {StatusCode = StatusCode.NotFound, ErrorMessage = "Friend request was not sent",};
            }

            from.FriendRequests.Remove(toUser);
            from.Friends.Add(toUser);
            to.FriendRequestsSent.Remove(fromUser);
            to.Friends.Add(fromUser);
            var updateFrom = await _userManager.UpdateUser(from.Id, from);
            var updateTo = await _userManager.UpdateUser(to.Id, to);
            if (!updateFrom || !updateTo)
            {
                return new AcceptResponse {StatusCode = StatusCode.Internal, ErrorMessage = "Internal error! Couldn't update users"};
            }

            return new AcceptResponse
            {
                StatusCode = StatusCode.Ok,
                ErrorMessage = $"Friend request of {to.Name} is accepted by {from.Name}",
            };
        }

        public async Task<RejectResponse> RejectFriendRequest(string fromUser, string toUser)
        {
            if (fromUser == toUser)
            {
                return new RejectResponse {StatusCode = StatusCode.InvalidArgument, ErrorMessage = "User can't reject request of himself/herself"};
            }
            var from = await this._userManager.GetUser(fromUser);
            var to = await this._userManager.GetUser(toUser);
            if (from == null || to == null)
            {
                return new RejectResponse {StatusCode = StatusCode.NotFound, ErrorMessage = "User not found",};
            }

            var fromCheck = from.FriendRequests.FirstOrDefault(e => e == toUser);
            if (fromCheck == null)
            {
                return new RejectResponse {StatusCode = StatusCode.NotFound, ErrorMessage = "Friend request not found",};
            }
            var toCheck = to.FriendRequestsSent.FirstOrDefault(e => e == fromUser);
            if (toCheck == null)
            {
                return new RejectResponse {StatusCode = StatusCode.NotFound, ErrorMessage = "Friend request was not sent",};
            }

            from.FriendRequests.Remove(toUser);
            to.FriendRequestsSent.Remove(fromUser);
            var updateFrom = await _userManager.UpdateUser(from.Id, from);
            var updateTo = await _userManager.UpdateUser(to.Id, to);
            if (!updateFrom || !updateTo)
            {
                return new RejectResponse {StatusCode = StatusCode.Internal, ErrorMessage = "Internal error! Couldn't update users"};
            }

            return new RejectResponse
            {
                StatusCode = StatusCode.Ok,
                ErrorMessage = $"Friend request of {to.Name} is rejected by {from.Name}",
            };
        }

        private UserResponse ConvertToUserResponse(User user)
        {
            if (user == null)
            {
                return null;
            }
            UserResponse userResponse = new UserResponse()
            {
                Id = user.Id, Name = user.Name, Email = user.Email, CreatedAt = user.CreatedAt,
            };
            return userResponse;
        }
    }
}