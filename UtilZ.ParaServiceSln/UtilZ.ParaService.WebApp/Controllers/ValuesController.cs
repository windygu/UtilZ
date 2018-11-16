using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAppRestFull.Controllers
{
    //[AcceptVerbs()]       
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        //https://localhost:44312/api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            ////登录授权直接跳转index界面
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    return RedirectToPage("Index");
            //}

            return new string[] { "value1", "value2" };
        }

        //// GET api/values/5
        ////https://localhost:44312/api/values/1
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value:" + id.ToString();
        //}

        // GET api/values/5
        //https://localhost:44312/api/values/abc
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

        ////https://localhost:44312/api/values/123&123
        //[HttpGet("{pid}&{sid}")]
        //public ActionResult<string> Get(long pid, long sid)
        //{
        //    return string.Format("pid:{0}, sid:{1}", pid, sid);
        //}

        //https://localhost:44312/api/values/pid=123&sid=123
        [HttpGet("pid={pid}&sid={sid}")]
        public ActionResult<string> Get(long pid, long sid)
        {
            return string.Format("pid:{0}, sid:{1}", pid, sid);
        }


        // GET api/values/5
        //https://localhost:44312/api/values/abc
        [HttpGet("{id}/{sid}/{ver}/{p}")]
        //[AcceptVerbs("Get", "Post")]     //同时支持get和post请求,用AcceptVerbs标识即可
        public ActionResult<string> Get(long pid, long sid, int ver, int p)
        {
            return "同时支持get和post请求,用AcceptVerbs标识即可 value:" + pid;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        }

        // POST api/values
        [HttpPost]
        public ActionResult<string> PostRet([FromBody] string value)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
            return "value:" + value;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        }
    }
}
