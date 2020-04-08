using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelsService.Models;

namespace AuthenticationService
{
    public static class UserExtension
    {
        private static string GetSecretKey()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            return configBuilder.GetValue<string>("JwtSettings:Secret");
        }
        public static string GetEncryptedPassword(string password)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: System.Text.Encoding.UTF8.GetBytes(GetSecretKey()),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
            ));
            return hashedPassword;
        }
        public static void SetPassword(this User user, string password)
        {
            var hashedPassword = GetEncryptedPassword(password);
            user.Password = hashedPassword;
        }
        public static bool CheckPassword(this User user, string password)
        {
            var hashedPassword = GetEncryptedPassword(password);
            if (user.Password == hashedPassword)
            {
                return true;
            }
            return false;
        }

        public static string GetToken(this User user)
        {
            var tokenHandler = JwtToken.GetHandler();
            var key = Encoding.ASCII.GetBytes(GetSecretKey());
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
        private static JwtSecurityTokenHandler _tokenHandler;

        public static JwtSecurityTokenHandler GetHandler()
        {
            if (_tokenHandler == null)
            {
                _tokenHandler = new JwtSecurityTokenHandler();
            }
            return _tokenHandler;
        }
    }

}