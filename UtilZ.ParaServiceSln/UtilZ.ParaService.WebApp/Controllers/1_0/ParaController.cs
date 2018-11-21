using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.ParaService.Model;
using UtilZ.ParaService.WebApp.Models;

namespace UtilZ.ParaService.WebApp.Controllers._1_0
{
    //[Authorize]
    [EnableCors(WebAppConstant.CorsPolicy)]//js跨域
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ParaController : ControllerBase
    {
        // GET: api/Para
        [HttpGet]
        public ActionResult<string> Get()
        {
            var userInfo = AuthenticationController.GetUserInfo(Request.Headers[WebAppConstant.AccessToken]);
            if (userInfo == null)
            {
                return Unauthorized();
            }


            return "value1";
        }

        // GET: api/Para/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(string id)
        {
            return "value";
        }

        // POST: api/Para
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Para/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
