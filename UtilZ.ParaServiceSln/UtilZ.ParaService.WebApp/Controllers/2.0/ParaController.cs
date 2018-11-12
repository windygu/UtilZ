using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UtilZ.ParaService.WebApp.Controllers.V2
{
    [Route("v2/api/[controller]")]
    [ApiController]
    public class ParaController : ControllerBase
    {
        // GET api/values/5
        //https://localhost:44312/v2/api/Para/werqwe
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
    }
}