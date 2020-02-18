using System;

namespace BlogWebApi.Contracts.v1.Requests.AuthRequest
{
    public class LoginUserRequest : IAuthUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                return false;
            }
            return true;
        }
    }
}