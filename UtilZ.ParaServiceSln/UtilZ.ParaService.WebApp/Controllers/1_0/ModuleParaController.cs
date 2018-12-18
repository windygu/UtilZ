using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UtilZ.ParaService.BLL;
using UtilZ.ParaService.Model;
using UtilZ.ParaService.WebApp.Models;

namespace UtilZ.ParaService.WebApp.Controllers._1_0
{
    [EnableCors(WebAppConstant.CorsPolicy)]//js跨域
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ModuleParaController : ControllerBase
    {
        private readonly ProjectBLL _bll;
        public ModuleParaController() : base()
        {
            this._bll = new ProjectBLL();
        }

        [HttpGet("projectId={projectId}&moduleId={moduleId}")]
        public ApiData Get(long projectId, long moduleId)
        {
            return this._bll.QueryModuleParas(projectId, moduleId);
        }
    }
}