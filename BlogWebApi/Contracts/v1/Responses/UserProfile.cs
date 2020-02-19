using System;
using ModelsProject;

namespace BlogWebApi.Contracts.v1.Responses
{
    public class UserProfile
    {
        public string Name{ get; set; }
        public string Email { get; set; }
        
        public UserProfile(User user)
        {
            this.Name = user.Name;
            this.Email = user.Email;
        }
    }
}