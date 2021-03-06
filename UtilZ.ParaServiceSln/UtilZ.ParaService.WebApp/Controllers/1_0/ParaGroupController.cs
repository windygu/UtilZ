﻿using System;
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
        [HttpGet("projectID={projectId}&pageSize={pageSize}&pageIndex={pageIndex}")]
        public ApiData Get(long projectId, int pageSize, int pageIndex)
        {
            return this._bll.QueryParaGroups(projectId, pageSize, pageIndex);
        }

        [HttpGet("id={id}")]
        public ApiData Query(long id)
        {
            return this._bll.QueryParaGroup(id);
        }

        // POST: api/ParaGroup
        [HttpPost]
        public ApiData Post([FromBody] ParaGroup paraGroup)
        {
            return this._bll.AddParaGroup(paraGroup);
        }

        // PUT: api/ParaGroup/5
        [HttpPut]
        public ApiData Put([FromBody] ParaGroup paraGroup)
        {
            return this._bll.UpdateParaGroup(paraGroup);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("projectId={projectId}&id={id}")]
        public ApiData Delete(long projectId, long id)
        {
            return this._bll.DeleteParaGroup(projectId, id);
        }
    }
}