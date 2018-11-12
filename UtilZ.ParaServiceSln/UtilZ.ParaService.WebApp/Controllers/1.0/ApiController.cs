using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UtilZ.ParaService.WebApp.Controllers.V1
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        // GET api/values/5
        //https://localhost:44312/v1/api
        [HttpGet]
        public ActionResult<string> Get()
        {
            return @"{
                ""link"": {
                    ""rel"":   ""collection https://www.example.com/zoos"",
      ""href"":  ""https://api.example.com/zoos"",
      ""title"": ""List of zoos"",
      ""type"":  ""application/vnd.yourformat+json""
               }
            }
            """;
        }
    }
}