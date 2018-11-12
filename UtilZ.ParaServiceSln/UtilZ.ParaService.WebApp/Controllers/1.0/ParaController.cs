using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UtilZ.ParaService.WebApp.Controllers.V1
{
    [EnableCors("CorsPolicy")]
    [Route("v1/api/[controller]")]
    [ApiController]
    public class ParaController : ControllerBase
    {
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