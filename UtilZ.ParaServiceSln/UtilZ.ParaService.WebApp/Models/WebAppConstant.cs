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

            //var paraValues = new List<ParaValue>();
            //paraValues.Add(new ParaValue() { ParaID = 1, ProjectID = 2, Value = "123" });
            //paraValues.Add(new ParaValue() { ParaID = 2, ProjectID = 2, Value = "192.168.10.96" });
            //string json = JsonConvert.SerializeObject(paraValues);

            //string str = "[{\"ParaID\":1, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"1\" },{\"ParaID\":3, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"2\" },{\"ParaID\":4, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"3\" },{\"ParaID\":5, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"4\" },{\"ParaID\":6, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"5\" },{\"ParaID\":7, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"6\" },{\"ParaID\":8, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"7\" },{\"ParaID\":11, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"8\" },{\"ParaID\":12, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"9\" },{\"ParaID\":14, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"0\" },{\"ParaID\":15, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"hj\" },{\"ParaID\":16, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"hfgj\" },{\"ParaID\":17, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"nm\" },{\"ParaID\":18, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"zcxv\" },{\"ParaID\":19, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"feg\" },{\"ParaID\":20, \"ProjectID\": 8, \"Version\": 0, \"Value\": \"vbfnmbvc\" }]";
            //var paraValues2 = JsonConvert.DeserializeObject<List<ParaValue>>(str);

            //var pvsp = new ParaValueSettingPost();
            //pvsp.PID = 123;
            //pvsp.ParaValueSettings.Add(new ParaValueSetting() { Id = 1, Value = "sadf" });
            //pvsp.ParaValueSettings.Add(new ParaValueSetting() { Id = 2, Value = "132" });
            //string json = JsonConvert.SerializeObject(pvsp);
        }
    }
}
