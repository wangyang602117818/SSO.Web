using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace SSO.Util
{
    public class JwtManager
    {
        public static string GenerateToken(string userId, string userName, string lang, string company, IEnumerable<string> departments, IEnumerable<string> roles, string ip, int minutes)
        {
            var symmetricKey = Convert.FromBase64String(AppSettings.secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>() { new Claim(ClaimTypes.Name, userId) };
            if (!string.IsNullOrEmpty(userName)) claims.Add(new Claim("StaffName", userName));
            if (!string.IsNullOrEmpty(lang)) claims.Add(new Claim("Lang", lang));
            if (!string.IsNullOrEmpty(company)) claims.Add(new Claim("Company", company));
            if (departments != null)
                foreach (string dept in departments) claims.Add(new Claim("Department", dept));
            if (roles != null)
                foreach (string role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),  //token数据
                Issuer = AppSettings.issuer,           //颁发者
                IssuedAt = DateTime.Now,               //颁发时间
                Audience = ip,                         //颁发给
                Expires = DateTime.Now.AddMinutes(minutes), //过期时间
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)   //签名
            };
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);
            return token;
        }
        public static string ModifyTokenLang(string token, string lang, int minutes)
        {
            var symmetricKey = Convert.FromBase64String(AppSettings.secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var stoken = tokenHandler.ReadJwtToken(token);
            var newClaims = new List<Claim>() { };
            foreach (var claim in stoken.Claims)
            {
                if (claim.Type == "Lang")
                {
                    newClaims.Add(new Claim("Lang", lang));
                }
                else
                {
                    newClaims.Add(new Claim(claim.Type, claim.Value));
                }
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(newClaims),  //token数据
                IssuedAt = DateTime.Now,               //颁发时间
                Expires = DateTime.Now.AddMinutes(minutes), //过期时间
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)   //签名
            };
            var newStoken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(newStoken);
        }
        public static string GenerateTicket(string userId)
        {
            string sourceString = DateTime.Now.ToString("yyyy-MM-dd") + userId + DateTime.Now.ToString("HH:mm:ss");
            string ticket = SymmetricEncryptHelper.AesEncode(sourceString, AppSettings.secretKey);
            return Base64SecureURL.Encode(ticket);
        }
        public static string DecodeTicket(string ticket)
        {
            string sourceString = SymmetricEncryptHelper.AesDecode(Base64SecureURL.Decode(ticket), AppSettings.secretKey);
            string userId = sourceString.Substring(10, sourceString.Length - 18);
            DateTime ticketTime = DateTime.Parse(sourceString.Substring(0, 10) + " " + sourceString.Substring(10 + userId.Length));
            var diff = DateTime.Now - ticketTime;
            if (diff.TotalSeconds > Convert.ToInt32(AppSettings.ticketTime)) return "";
            return userId;
        }
    }
}
