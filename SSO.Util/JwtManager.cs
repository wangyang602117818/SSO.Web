using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SSO.Util
{
    public class JwtManager
    {
        private static string secretKey = ConfigurationManager.AppSettings["secretKey"];
        private static string issuer = ConfigurationManager.AppSettings["issuer"];
        public static string GenerateToken(string userId, string userName, IEnumerable<string> roles, string ip)
        {
            var symmetricKey = Convert.FromBase64String(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userId),
                new Claim("StaffName",userName)
            };
            foreach (string role in roles) claims.Add(new Claim("Role", role));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),  //token数据
                Issuer = issuer,                       //颁发者
                IssuedAt = DateTime.Now,               //颁发时间
                Audience = ip,                         //颁发给
                Expires = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59), //过期时间
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)   //签名
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
