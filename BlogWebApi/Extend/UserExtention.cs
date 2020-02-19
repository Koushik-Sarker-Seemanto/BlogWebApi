using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ModelsProject;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BlogWebApi.Extend
{
    public static class UserExtention
    {

        public static string GetEncrypedPassword(string password)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: System.Text.Encoding.UTF8.GetBytes(AppSettingsProvider.jwtSettings.Secret),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
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
            if (user.Password == hashedPassword)
            {
                return true;
            }
            return false;
        }

        public static string GetToken(this User user)
        {
            var tokenHandler = JwtToken.GetHandler();
            var key = Encoding.ASCII.GetBytes(AppSettingsProvider.jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }

    class JwtToken
    {
        private static JwtSecurityTokenHandler tokenHandler;

        public static JwtSecurityTokenHandler GetHandler()
        {
            if (tokenHandler == null)
            {
                tokenHandler = new JwtSecurityTokenHandler();
            }
            return tokenHandler;
        }
    }
}
