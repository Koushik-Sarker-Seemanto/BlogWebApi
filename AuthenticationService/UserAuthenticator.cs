using System.Threading.Tasks;
using ContractsService;
using ContractsService.v1.UserContracts.Requests;
using ContractsService.v1.UserContracts.Responses;
using ModelsService.Managers.UserManager;
using ModelsService.Models;

namespace AuthenticationService
{
    /// <summary>
    /// This class authenticates user.
    /// </summary>
    public class UserAuthenticator : IUserAuthenticator
    {
        private readonly IUserManager _userManager;
        
        /// <summary>
        /// instantiate UserManager class.
        /// </summary>
        /// <param name="userManager"></param>
        public UserAuthenticator(IUserManager userManager)
        {
            _userManager = userManager;
        }
        
        /// <summary>
        /// This method is used to make user login.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>login response string.</returns>
        public async Task<string> LoginUser(LoginRequest request)
        {
            var user = await _userManager.GetUserByEmail(request.Email);
            if(user == null)
            {
                return StatusCode.NotFound.ToString();
            }
            if(user.CheckPassword(request.Password) == false)
            {
                return StatusCode.InvalidArgument.ToString();
            }
            return user.GetToken();
        }

        /// <summary>
        /// This method is used to make user register.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>register response string.</returns>
        public async Task<string> RegisterUser(RegisterRequest request)
        {
            var existingUser = await _userManager.GetUserByEmail(request.Email);
            if(existingUser != null)
            {
                return StatusCode.AlreadyExists.ToString();
            }
            if(!request.IsValid())
            {
                return StatusCode.InvalidArgument.ToString();
            }

            var newUser = new User
            {
                Email = request.Email, Name = request.Name
            };
            newUser.SetPassword(request.Password);
            var added = await _userManager.InsertUser(newUser);
            if (!added)
            {
                return StatusCode.Internal.ToString();
            }

            return newUser.GetToken();
        }

        /// <summary>
        /// This method returns the logged-in user profile.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>profile.</returns>
        public async Task<ProfileResponse> ReturnProfile(string context)
        {
            var user = await _userManager.GetUser(context);
            
            if(user == null)
            {
                return null;
            }
            
            ProfileResponse profileResponse = new ProfileResponse()
            {
                Name = user.Name,
                Email = user.Email,
            };
            return profileResponse;
        }
    }
}