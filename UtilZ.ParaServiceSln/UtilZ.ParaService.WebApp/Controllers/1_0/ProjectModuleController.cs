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
    [EnableCors(WebAppConstant.CorsPolicy)]//js跨域
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectModuleController : ControllerBase
    {
        private readonly ProjectBLL _bll;
        public ProjectModuleController() : base()
        {
            this._bll = new ProjectBLL();
        }

        // GET: api/ProjectModule
        //[HttpGet]
        //[HttpGet("{pageSize}/{pageIndex}")]
        [HttpGet("projectID={projectID}&pageSize={pageSize}&pageIndex={pageIndex}")]
        public ApiData Get(long projectId, int pageSize, int pageIndex)
        {
            return this._bll.QueryProjectModules(projectId, pageSize, pageIndex);
        }

        [HttpGet("id={id}")]
        public ApiData Query(long id)
        {
            return this._bll.QueryProjectModule(id);
        }

        // POST: api/ProjectModule
        [HttpPost]
        public ApiData Post([FromBody] ProjectModule projectModule)
        {
            return this._bll.AddProjectModule(projectModule);
        }

        // PUT: api/ProjectModule/5
        [HttpPut]
        public ApiData Put([FromBody] ProjectModule projectModule)
        {
            return this._bll.UpdateProjectModule(projectModule);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("id={id}")]
        public ApiData Delete(long id)
        {
            return this._bll.DeleteProjectModule(id);
        }
    }
}
