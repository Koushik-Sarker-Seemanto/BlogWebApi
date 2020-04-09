using System.Threading.Tasks;
using ContractsService.v1.UserContracts.Requests;
using ContractsService.v1.UserContracts.Responses;

namespace AuthenticationService
{
    public interface IUserAuthenticator
    {
        public Task<LoginResponse> LoginUser(LoginRequest request);
        public Task<RegisterResponse> RegisterUser(RegisterRequest request);
        public Task<ProfileResponse> ReturnProfile(string context);
        public Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, string context);
    }
}