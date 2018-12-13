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

        // GET: api/ParaValue
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ParaValue/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Project
        [HttpPost]
        public ApiData Post([FromBody] ParaValueSettingPara para)
        {
            return this._bll.SetParaValue(para.ToParaValues());
        }

        //// POST: api/ParaValue
        //[HttpPost]
        //public ApiData Post([FromBody] string value)
        //{

        //    var paraValues = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ParaValue>>(value);
        //    return this._bll.SetParaValue(paraValues);
        //}

        //[HttpPost]
        //public ApiData SetParaValue([FromBody] List<ParaValue> paraValues)
        //{
        //    return this._bll.SetParaValue(paraValues);
        //}

        // PUT: api/ParaValue/5
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
