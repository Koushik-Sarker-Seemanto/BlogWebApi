using System;
using BlogWebApi.Contracts.v1.Requests.AuthRequest;

namespace BlogWebApi.Contracts.v1.Requests.AuthRequest
{
    public class RegisterUserRequest : IAuthUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Name))
            {
                return false;
            }
            return true;
        }
    }
}