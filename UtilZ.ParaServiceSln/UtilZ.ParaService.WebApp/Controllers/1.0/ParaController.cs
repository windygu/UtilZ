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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using UtilZ.ParaService.WebApp.Models;

namespace UtilZ.ParaService.WebApp.Controllers.V1
{
    [Authorize]
    [EnableCors("CorsPolicy")]
    [Route("v1/api/[controller]")]
    [ApiController]
    public class ParaController : ControllerBase
    {
        public ParaController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // GET api/values/5
        //https://localhost:44312/v1/api/Para/werqwe
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            return "string value:" + id;
        }

        //https://localhost:44312/api/values/1/2/3
        [HttpGet("{pid}/{sid}/{ver}")]
        public ActionResult<string> Get(long pid, long sid, int ver)
        {
            return string.Format("pid:{0}, sid:{1}, ver:{2}", pid, sid, ver);
        }

        //[HttpPost]
        //public ActionResult<string> Post([FromBody] string value)
        //{
        //    // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        //    return "value:" + value;
        //}

        // POST api/values
        //[HttpPost("{value}")]
        //public ActionResult<string> Post([FromBody] string value)
        //{
        //    //StatusCode(123);
        //    //Ok();
        //    //return CreatedAtRoute("Get", null);
        //    return "value:" + value;
        //}

        //[HttpPost]
        //public ActionResult<string> Post([FromBody] string value)
        //{
        //    //StatusCode(123);
        //    //Ok();
        //    //return CreatedAtRoute("Get", null);
        //    return "value:" + value;
        //}

        [HttpPost]
        public ActionResult<string> Post([FromBody] login value)
        {
            //StatusCode(123);
            //Ok();
            //return CreatedAtRoute("Get", null);

            return "value:" + value.ToString();


            //var user = _store.FindUser(userDto.UserName, userDto.Password);
            //if (user == null)
            //{
            //    return Unauthorized();
            //}

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(ParaServiceConstant.Secret);
            //var authTime = DateTime.UtcNow;
            //var expiresAt = authTime.AddDays(7);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(JwtClaimTypes.Audience,"api"),
            //        new Claim(JwtClaimTypes.Issuer,"http://localhost:5200"),
            //        new Claim(JwtClaimTypes.Id,"Id"),
            //        new Claim(JwtClaimTypes.Name,"Name"),
            //        new Claim(JwtClaimTypes.Email, "Email"),
            //        new Claim(JwtClaimTypes.PhoneNumber, "PhoneNumber")
            //    }),
            //    Expires = expiresAt,
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //var tokenString = tokenHandler.WriteToken(token);
            //return Ok(new
            //{
            //    access_token = tokenString,
            //    token_type = "Bearer",
            //    profile = new
            //    {
            //        sid = user.Id,
            //        name = user.Name,
            //        auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
            //        expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
            //    }
            //});
        }

        //[HttpPost]
        //public IActionResult Post()
        //{
        //    StringValues sv = Request.Headers["Authorization"];
        //    if (sv.Count == 0)
        //    {
        //        return Unauthorized();
        //    }

        //    var authorizationHeader = sv.First();
        //    var key = authorizationHeader.Split(' ')[1];
        //    var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(key)).Split(':');
        //    var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:ServerSecret"]));
        //    if (credentials[0] == "username" && credentials[1] == "password")
        //    {
        //        var result = new
        //        {
        //            token = GenerateToken(serverSecret)
        //        };

        //        return Ok(result);
        //    }
        //    return BadRequest();
        //}

        private string GenerateToken(SecurityKey key)
        {
            var now = DateTime.UtcNow;
            var issuer = Configuration["JWT:Issuer"];
            var audience = Configuration["JWT:Audience"];
            var identity = new ClaimsIdentity();
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(issuer, audience, identity, now, now.Add(TimeSpan.FromHours(1)), now, signingCredentials);
            var encodedJwt = handler.WriteToken(token);
            return encodedJwt;
        }


        //[HttpPost]
        //public string Post([FromBody]WLCode wlcode)
        //{
        //}


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class login
    {
        public string action { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public override string ToString()
        {
            return action;
        }
    }
}