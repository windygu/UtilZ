using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilZ.ParaService.DBModel;

namespace UtilZ.ParaService.WebApp.Models
{
    public class WebAppConstant
    {
        /// <summary>
        /// //js跨域
        /// </summary>
        public const string CorsPolicy = "CorsPolicy";

        public const string Secret = "JwtBearerSample_11231~#$%#%^2235";

        public const string AccessToken = "access_token";

        public static void Test()
        {
            //Project project = new Project();
            //project.ID = 0;
            //project.Name = "无线电大数据处理系统";
            //project.Alias = "RDPS";
            //project.Des = "中卫市_无线电大数据处理系统";
            //string json = JsonConvert.SerializeObject(project);

            var paraValues = new List<ParaValue>();
            paraValues.Add(new ParaValue() { ParaID = 1, ProjectID = 2, Value = "123" });
            paraValues.Add(new ParaValue() { ParaID = 2, ProjectID = 2, Value = "192.168.10.96" });
            string json = JsonConvert.SerializeObject(paraValues);
        }
    }
}
