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
using UtilZ.ParaService.DBModel;
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

        // GET: api/Para/5
        [HttpGet("projectId ={projectId}&paraGroupId={paraGroupId}")]
        public ApiData Get(long projectId, long paraGroupId, int pageSize, int pageIndex)
        {
            return this._bll.QueryParas(projectId, paraGroupId, pageSize, pageIndex);
        }

        [HttpGet("id={id}")]
        public ApiData Query(long id)
        {
            return this._bll.QueryPara(id);
        }

        // POST: api/Para
        [HttpPost]
        public ApiData Post([FromBody] Para para)
        {
            return this._bll.AddPara(para);
        }

        // PUT: api/Para/5
        [HttpPut]
        public ApiData Put([FromBody] Para para)
        {
            return this._bll.UpdatePara(para);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("id={id}")]
        public ApiData Delete(int id)
        {
            return this._bll.DeletePara(id);
        }
    }
}
