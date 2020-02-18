using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ModelsProject;


namespace BlogWebApi.Extend
{
    public static class UserExtention
    {

        public static string GetEncrypedPassword(string password)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password : password,
                salt : System.Text.Encoding.UTF8.GetBytes(AppSettingsProvider.jwtSettings.Secret),
                prf : KeyDerivationPrf.HMACSHA256,
                iterationCount : 1000,
                numBytesRequested: 256/8
            ));
            return hashedPassword;
        }
        public static void SetPassword(this User user, string password)
        {
            var hashedPassword = GetEncrypedPassword(password);
            user.Password = hashedPassword;
        }
        public static bool CheckPassword(this User user, string password)
        {
            var hashedPassword = GetEncrypedPassword(password);
            if(user.Password == hashedPassword)
            {
                return true;
            }
            return false;
        }
    }
}
