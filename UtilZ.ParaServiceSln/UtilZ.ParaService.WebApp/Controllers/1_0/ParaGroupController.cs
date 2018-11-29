using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UtilZ.ParaService.BLL;
using UtilZ.ParaService.DBModel;
using UtilZ.ParaService.WebApp.Models;

namespace UtilZ.ParaService.WebApp.Controllers._1_0
{
    [EnableCors(WebAppConstant.CorsPolicy)]//js跨域
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ParaGroupController : ControllerBase
    {
        private readonly ProjectBLL _bll;
        public ParaGroupController() : base()
        {
            this._bll = new ProjectBLL();
        }

        // GET: api/ParaGroup
        //[HttpGet]
        //[HttpGet("{pageSize}/{pageIndex}")]
        [HttpGet("projectID={projectID}&pageSize={pageSize}&pageIndex={pageIndex}")]
        public IEnumerable<ParaGroup> Get(long projectID, int pageSize, int pageIndex)
        {
            return this._bll.QueryParaGroups(projectID, pageSize, pageIndex);
        }

        [HttpGet("id={id}")]
        public ParaGroup Query(long id)
        {
            return this._bll.QueryParaGroup(id);
        }

        // POST: api/ParaGroup
        [HttpPost]
        public long Post([FromBody] ParaGroup paraGroup)
        {
            return this._bll.AddParaGroup(paraGroup);
        }

        // PUT: api/ParaGroup/5
        [HttpPut]
        public int Put([FromBody] ParaGroup paraGroup)
        {
            return this._bll.UpdateParaGroup(paraGroup);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("id={id}")]
        public int Delete(long id)
        {
            return this._bll.DeleteParaGroup(id);
        }
    }
}