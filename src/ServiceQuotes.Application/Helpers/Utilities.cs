using Microsoft.IdentityModel.Tokens;
using System;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace ServiceQuotes.Application.Helpers
{
    public class Utilities
    {
        public static string GenerateJwtToken(Guid accountId, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", accountId.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static JwtSecurityToken ValidateJwtToken(string token, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }

        public static string HashPassword(string password)
        {
            return BC.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string passwordHash)
        {
            return BC.Verify(password, passwordHash);
        }

        public static bool IsValidImage(byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    Image.FromStream(ms);
                }
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        public static string CalculateHMAC(byte[] key, byte[] data)
        {
            var hmacsha256 = new HMACSHA256(key);
            byte[] hashmessage = hmacsha256.ComputeHash(data);
            return Convert.ToBase64String(hashmessage);
        }
    }
}
