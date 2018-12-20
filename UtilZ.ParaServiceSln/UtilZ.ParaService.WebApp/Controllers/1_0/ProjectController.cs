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
    public class ProjectController : BasicController
    {
        //GET（SELECT）：从服务器取出资源（一项或多项）。
        //POST（CREATE）：在服务器新建一个资源。
        //PUT（UPDATE）：在服务器更新资源（客户端提供改变后的完整资源）。
        //PATCH（UPDATE）：在服务器更新资源（客户端提供改变的属性）。
        //DELETE（DELETE）：从服务器删除资源

        private readonly ProjectBLL _bll;
        public ProjectController() : base()
        {
            this._bll = new ProjectBLL();
        }

        // GET: api/Project
        //[HttpGet]
        //[HttpGet("{pageSize}/{pageIndex}")]
        [HttpGet("pageSize={pageSize}&pageIndex={pageIndex}")]
        public ApiData Get(int pageSize, int pageIndex)
        {
            //var token = this.Request.Headers[WebAppConstant.AccessToken];
            //var userInfo = AuthenticationController.GetUserInfo(token);

            return this._bll.QueryProjects(pageSize, pageIndex);
        }

        [HttpGet("id={id}")]
        public ApiData Query(long id)
        {
            return this._bll.QueryProject(id);
        }

        // POST: api/Project
        [HttpPost]
        public ApiData Post([FromBody] Project project)
        {
            return this._bll.AddProject(project);
        }

        // PUT: api/Project/5
        [HttpPut]
        public ApiData Put([FromBody] Project project)
        {
            return this._bll.UpdateProject(project);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("id={id}")]
        public ApiData Delete(long id)
        {
            return this._bll.DeleteProject(id);
        }
    }
}
