using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.ParaService.Model;
using UtilZ.ParaService.WebApp.Models;

namespace UtilZ.ParaService.WebApp.Controllers._1_0
{
    [EnableCors(WebAppConstant.CorsPolicy)]//js跨域
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            string userName = Request.Headers["userName"];
            string password = Request.Headers["password"];

            if (!string.Equals(userName, "admin", StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(password, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized();
            }

            var userInfo = new UserInfo();
            userInfo.Email = "yf@163.com";
            userInfo.Id = 1;
            userInfo.Name = userName;
            userInfo.Password = password;
            userInfo.PhoneNumber = "13709084809";
            userInfo.RoleID = 1;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(WebAppConstant.Secret);
            var authTime = DateTime.Now;
            var expiresAt = authTime.AddMilliseconds(WebAppConfig.Instance.TokenExpireTime);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Audience,"api"),
                    new Claim(JwtClaimTypes.Issuer,"http://localhost:5200"),
                    new Claim(JwtClaimTypes.Id, userInfo.Id.ToString()),
                    new Claim(JwtClaimTypes.Name, userInfo.Name),
                    new Claim(JwtClaimTypes.Email, userInfo.Email),
                    new Claim(JwtClaimTypes.PhoneNumber, userInfo.PhoneNumber)
                }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            UpdateToken(tokenString, userInfo);
            //WebAppConstant.AccessToken="access_token";
            return Ok(new
            {
                access_token = tokenString,
                token_type = "Bearer",
                profile = new
                {
                    sid = userInfo.Id,
                    name = userInfo.Name,
                    auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                    expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
                }
            });
        }

        private static void UpdateToken(string token, UserInfo userInfo)
        {
            MemoryCacheEx.Set(token, userInfo, WebAppConfig.Instance.TokenExpireTime);
        }

        public static UserInfo GetUserInfo(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var userInfo = MemoryCacheEx.Get(token) as UserInfo;
            if (userInfo == null)
            {
                return null;
            }

            MemoryCacheEx.Set(token, userInfo, WebAppConfig.Instance.TokenExpireTime);
            return userInfo;
        }
    }
}