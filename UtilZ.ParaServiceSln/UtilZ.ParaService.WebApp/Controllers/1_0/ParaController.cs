using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.ParaService.BLL;
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
        private readonly ProjectBLL _bll;
        public ParaController() : base()
        {
            this._bll = new ProjectBLL();
        }

        //// GET: api/Para
        //[HttpGet]
        //public ActionResult<string> Get()
        //{
        //    var userInfo = AuthenticationController.GetUserInfo(Request.Headers[WebAppConstant.AccessToken]);
        //    if (userInfo == null)
        //    {
        //        return Unauthorized();
        //    }


        //    return "value1";
        //}

        // GET: api/Para/5
        [HttpGet("projectId ={projectId}&paraGroupId={paraGroupId}")]
        public ApiData Get(long projectId, long paraGroupId, int pageSize, int pageIndex)
        {
            return this._bll.QueryParas(projectId, paraGroupId, pageSize, pageIndex);
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
