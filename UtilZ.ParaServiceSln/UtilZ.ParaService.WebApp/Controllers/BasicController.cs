using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UtilZ.ParaService.DBModel;
using UtilZ.ParaService.WebApp.Models;

namespace UtilZ.ParaService.WebApp.Controllers
{
    //[EnableCors(WebAppConstant.CorsPolicy)]//js跨域
    //[Route("api/v1/[controller]")]
    [ApiController]
    public class BasicController : ControllerBase
    {
        public BasicController() : base()
        {

        }
    }
}
