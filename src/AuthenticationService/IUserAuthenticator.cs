using System.Threading.Tasks;
using ContractsService.v1.UserContracts.Requests;
using ContractsService.v1.UserContracts.Responses;

namespace AuthenticationService
{
    public interface IUserAuthenticator
    {
        public Task<string> LoginUser(LoginRequest request);
        public Task<string> RegisterUser(RegisterRequest request);
        public Task<ProfileResponse> ReturnProfile(string context);
    }
}