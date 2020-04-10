using System;
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
        public async Task<LoginResponse> LoginUser(LoginRequest request)
        {
            var user = await _userManager.GetUserByEmail(request.Email);
            if (!request.IsValid())
            {
                return new LoginResponse()
                {
                    StatusCode = StatusCode.InvalidArgument,
                    ErrorMessage = "Empty argument provided",
                };
            }
            if(user == null)
            {
                return new LoginResponse()
                {
                    StatusCode = StatusCode.NotFound,
                    ErrorMessage = "User not found",
                };
            }
            if(user.CheckPassword(request.Password) == false)
            {
                return new LoginResponse()
                {
                    StatusCode = StatusCode.InvalidArgument,
                    ErrorMessage = "Wrong password",
                };
            }

            return new LoginResponse()
            {
                Token = user.GetToken(),
                StatusCode = StatusCode.Ok
            };
        }

        /// <summary>
        /// This method is used to make user register.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>register response string.</returns>
        public async Task<RegisterResponse> RegisterUser(RegisterRequest request)
        {
            var existingUser = await _userManager.GetUserByEmail(request.Email);

            if(existingUser != null)
            {
                return new RegisterResponse()
                {
                    StatusCode = StatusCode.AlreadyExists,
                    ErrorMessage = "Email already exist",
                };
            }
            if(!request.IsValid())
            {
                return new RegisterResponse()
                {
                    StatusCode = StatusCode.InvalidArgument,
                    ErrorMessage = "Empty argument provided",
                };
            }

            var newUser = new User
            {
                Email = request.Email, Name = request.Name
            };
            newUser.SetPassword(request.Password);
            var added = await _userManager.InsertUser(newUser);
            if (!added)
            {
                return new RegisterResponse()
                {
                    StatusCode = StatusCode.Internal,
                    ErrorMessage = "Internal error! Couldn't insert user"
                };
            }

            return new RegisterResponse()
            {
                Token = newUser.GetToken(),
                StatusCode = StatusCode.Ok,
            };
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
                return new ProfileResponse()
                {
                    StatusCode = StatusCode.NotFound,
                    ErrorMessage = "User not found",
                };
            }
            
            ProfileResponse profileResponse = new ProfileResponse()
            {
                StatusCode = StatusCode.Ok,
                User = new UserResponse()
                {
                    Id = user.Id, Name = user.Name, Email = user.Email, CreatedAt = user.CreatedAt,
                }
            };
            return profileResponse;
        }

        public async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, string context)
        {
            var currentUser = await _userManager.GetUser(context);
            if (currentUser == null)
            {
                return new UpdateUserResponse()
                {
                    StatusCode = StatusCode.Unauthenticated,
                    ErrorMessage = "Unauthenticated user",
                };
            }

            if (!request.IsValid())
            {
                return new UpdateUserResponse()
                {
                    StatusCode = StatusCode.InvalidArgument,
                    ErrorMessage = "Empty argument provided",
                };
            }

            var unavailableEmail = await _userManager.GetUserByEmail(request.Email);
            if (unavailableEmail != null)
            {
                return new UpdateUserResponse()
                {
                    StatusCode = StatusCode.AlreadyExists,
                    ErrorMessage = "This Email already exists",
                };
            }
            
            User user = new User()
            {
                Id = currentUser.Id,
                Name = request.Name,
                Email = request.Email,
            };
            user.SetPassword(request.Password);

            var update = await _userManager.UpdateUser(currentUser.Id, user);
            if (!update)
            {
                return new UpdateUserResponse()
                {
                    StatusCode = StatusCode.Internal,
                    ErrorMessage = "Internal Error! Couldn't update user",
                };
            }
            return new UpdateUserResponse()
            {
                Name = user.Name,
                Email = user.Email,
                StatusCode = StatusCode.Ok,
            };
        }
    }
}