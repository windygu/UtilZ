using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UtilZ.ParaService.BLL;
using UtilZ.ParaService.DBModel;
using UtilZ.ParaService.Model;
using UtilZ.ParaService.WebApp.Models;

namespace UtilZ.ParaService.WebApp.Controllers._1_0
{
    [EnableCors(WebAppConstant.CorsPolicy)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ParaValueController : ControllerBase
    {
        private readonly ProjectBLL _bll;
        public ParaValueController() : base()
        {
            this._bll = new ProjectBLL();
        }

        [HttpGet("projectID={projectId}&moduleId={moduleId}&version={version}")]
        public ApiData Get(long projectId, long moduleId, long version)
        {
            return this._bll.QueryParaValues(projectId, moduleId, version);
        }

        [HttpGet("projectAlias={projectAlias}&moduleAlias={moduleAlias}&version={version}")]
        public ApiData Get(string projectAlias, string moduleAlias, long version)
        {
            return this._bll.QueryParaValues(projectAlias, moduleAlias, version);
        }

        [HttpGet("projectID={projectId}&version={version}")]
        public ApiData Get(long projectId, long version)
        {
            return this._bll.QueryParaValues(projectId, version);
        }

        [HttpGet("projectAlias={projectAlias}&version={version}")]
        public ApiData Get(string projectAlias, long version)
        {
            return this._bll.QueryParaValues(projectAlias, version);
        }

        // POST: api/Project
        [HttpPost]
        public ApiData Post([FromBody] ParaValueSettingPost para)
        {
            return this._bll.SetParaValue(para);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("projectId={projectId}&beginVer={beginVer}&endVer={endVer}")]
        public ApiData Delete(long projectId, long beginVer, long endVer)
        {
            return this._bll.DeleteParaValue(projectId, beginVer, endVer);
        }
    }
}
